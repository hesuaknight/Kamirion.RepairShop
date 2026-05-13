# SKILL: Arquitectura de Tests Unitarios
# Archivo: .claude/rules/unit-testing.md
# Activar cuando: se solicitan o generan tests unitarios

---

## ROL ACTIVO

Al leer este skill, asumir el rol de **Arquitecto de Pruebas Unitarias**.

El objetivo es producir tests coherentes, mantenibles y con valor real.
No se generan tests por cantidad. Se generan tests por valor.

---

## PRINCIPIO CENTRAL

**Un test unitario existe para verificar una regla de negocio o invariante específico, no para alcanzar cobertura de código.**

Preguntas a responder antes de escribir un test:
1. ¿Qué comportamiento estoy verificando?
2. ¿Qué podría romperse si este test no existiera?
3. ¿Este test fallaría si introduzco el bug que intenta prevenir?

Si no se puede responder las tres preguntas, no escribir el test.

---

## QUÉ TESTEAR

### SIEMPRE testear (alta prioridad)

- **Invariantes de agregados:** reglas que el dominio debe hacer cumplir siempre
- **Transiciones de estado del workflow:** qué transiciones son válidas e inválidas
- **Lógica de `Result<T>` y errores:** caminos de falla en Application Layer
- **Value Objects:** validación, igualdad, normalización
- **Reglas de negocio complejas:** cálculos de costos, SLA, priorización

### TESTEAR con criterio (prioridad media)

- **Command Handlers:** solo cuando tienen lógica de orquestación no trivial
- **Domain Services:** cuando encapsulan reglas que involucran múltiples agregados
- **Factory methods:** cuando tienen validaciones no triviales

### NO testear (bajo o nulo valor)

- Constructores triviales sin lógica
- Propiedades get/set sin lógica
- Métodos que solo delegan a un repositorio sin lógica adicional
- Código generado automáticamente (migraciones, scaffolding)
- Configuración de DI
- Tests que solo verifican que un mock fue llamado (sin verificar comportamiento)

---

## ESTRUCTURA DE PROYECTOS DE TEST

```
tests/
  Kamirion.RepairShop.Architecture.Tests/    ← tests de arquitectura (ya definido)
  Kamirion.RepairShop.Shared.Tests/          ← tests de Shared Kernel (ya definido)
  Kamirion.RepairShop.{Module}.Tests/        ← uno por módulo, solo si hay lógica a testear
```

**Regla:** No crear un proyecto de tests para un módulo si no hay al menos 3 casos de test con valor real. Agregar al módulo existente más cercano o esperar a que haya masa crítica.

---

## CONVENCIÓN DE NAMING

### Nombre del archivo de test

```
{ClaseTesteada}Tests.cs

Ejemplos:
  RepairTicketTests.cs
  EstimateTests.cs
  MoneyTests.cs
  ChangeRepairTicketStatusCommandHandlerTests.cs
```

### Nombre del método de test

```
{Método}_{Escenario}_{ResultadoEsperado}

Ejemplos:
  ChangeStatus_WhenTransitionIsValid_ShouldUpdateStatus
  ChangeStatus_WhenTicketIsCancelled_ShouldReturnFailure
  Approve_WhenEstimateAlreadyApproved_ShouldReturnConflictError
  Create_WhenAmountIsNegative_ShouldThrowDomainException
```

---

## PATRÓN OBLIGATORIO: AAA (Arrange / Act / Assert)

```csharp
[Fact]
public void ChangeStatus_WhenTransitionIsValid_ShouldUpdateStatus()
{
    // Arrange
    var ticket = RepairTicketFactory.CreateInStatus(RepairStatus.Diagnosing);

    // Act
    var result = ticket.ChangeStatus(RepairStatus.WaitingApproval);

    // Assert
    result.IsSuccess.Should().BeTrue();
    ticket.Status.Should().Be(RepairStatus.WaitingApproval);
    ticket.DomainEvents.Should().ContainSingle(e => e is RepairTicketStatusChangedEvent);
}
```

Reglas del patrón:
- Comentarios `// Arrange`, `// Act`, `// Assert` son obligatorios
- Un solo `Act` por test
- Assertions agrupadas al final, nunca mezcladas con Act
- Un test verifica una sola cosa (puede tener múltiples assertions relacionadas)

---

## FACTORIES DE TEST (obligatorias)

Cada módulo con tests debe tener una carpeta `Factories/` con factories para construir entidades en estados conocidos. **Nunca construir entidades de dominio inline en cada test.**

```csharp
// tests/Kamirion.RepairShop.RepairWorkflow.Tests/Factories/RepairTicketFactory.cs
public static class RepairTicketFactory
{
    public static RepairTicket CreateDefault() =>
        RepairTicket.Create(
            tenantId: "tenant-test-001",
            branchId: "branch-test-001",
            customerId: "customer-test-001",
            deviceId: "device-test-001",
            issueDescription: "Test issue description"
        ).Value;

    public static RepairTicket CreateInStatus(RepairStatus status)
    {
        var ticket = CreateDefault();
        // avanzar al estado requerido...
        return ticket;
    }

    public static RepairTicket CreateCompleted() => CreateInStatus(RepairStatus.Completed);
    public static RepairTicket CreateCancelled() => CreateInStatus(RepairStatus.Cancelled);
}
```

---

## HERRAMIENTAS PERMITIDAS

```xml
<!-- Siempre presentes -->
<PackageReference Include="xunit" />
<PackageReference Include="FluentAssertions" />
<PackageReference Include="Microsoft.NET.Test.Sdk" />

<!-- Para mocks (solo en Application Layer tests) -->
<PackageReference Include="NSubstitute" />

<!-- Prohibidos -->
<!-- Moq: no usar, preferir NSubstitute -->
<!-- AutoFixture: no usar, preferir factories explícitas -->
```

---

## REGLAS DE MOCKING

- **Domain Layer:** sin mocks. Los tests de dominio son puros, sin dependencias externas.
- **Application Layer:** mockear repositorios e interfaces externas con NSubstitute.
- **Nunca mockear:** Value Objects, entidades de dominio, `Result<T>`, clases del Shared Kernel.

```csharp
// CORRECTO — mock de repositorio en Application test
var repository = Substitute.For<IRepairTicketRepository>();
repository.GetByIdAsync(ticketId, Arg.Any<CancellationToken>())
          .Returns(RepairTicketFactory.CreateDefault());

// INCORRECTO — mockear la entidad de dominio
var ticket = Substitute.For<RepairTicket>(); // PROHIBIDO
```

---

## TESTS DE TRANSICIONES DE ESTADO (obligatorios para workflow)

Para `RepairTicket` y cualquier agregado con estado, deben existir tests que cubran:

1. **Happy path:** cada transición válida definida en el workflow
2. **Invalid transitions:** intentar transiciones inválidas desde cada estado
3. **Domain events:** verificar que el evento correcto se publica en cada transición
4. **Invariants post-transición:** verificar el estado del agregado después de la transición

```csharp
// Ejemplo de test de transición inválida
[Fact]
public void ChangeStatus_WhenTicketIsCompleted_ShouldNotAllowReopen()
{
    // Arrange
    var ticket = RepairTicketFactory.CreateCompleted();

    // Act
    var result = ticket.ChangeStatus(RepairStatus.InRepair);

    // Assert
    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("RepairTicket.InvalidTransition");
}
```

---

## COBERTURA — FILOSOFÍA

- **No existe un target de cobertura porcentual.** La cobertura es una consecuencia, no un objetivo.
- Un 40% de cobertura con tests de valor real es mejor que un 90% con tests triviales.
- Si un test solo existe para subir el porcentaje de cobertura, eliminarlo.

---

## CHECKLIST AL GENERAR TESTS

Antes de dar la tarea por finalizada, verificar:

- [ ] Cada test tiene nombre en formato `{Método}_{Escenario}_{ResultadoEsperado}`
- [ ] Cada test sigue el patrón AAA con comentarios explícitos
- [ ] Los tests de dominio no tienen dependencias externas ni mocks
- [ ] Existe una Factory para las entidades usadas en los tests
- [ ] No se generaron tests triviales (solo getters/setters, constructores sin lógica)
- [ ] Los tests de workflow cubren tanto happy path como transiciones inválidas
- [ ] Se verifica publicación de Domain Events donde corresponde
- [ ] `dotnet test` pasa en verde con los tests nuevos
