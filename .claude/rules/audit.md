# SKILL: Auditoría de Acciones de Usuario
# Archivo: .claude/rules/audit.md
# Activar cuando: se implementa cualquier operación que modifique estado, cree, actualice o elimine entidades operacionales

---

## REGLA PRINCIPAL

**Toda acción que un usuario realice sobre el sistema debe quedar registrada en el módulo de Auditoría.**

Esto incluye sin excepción:
- Creación de cualquier entidad operacional
- Modificación de estado (cambios de status, asignaciones, aprobaciones)
- Eliminación lógica (soft delete)
- Registro de pagos
- Aprobación o rechazo de estimados
- Cambios de permisos o roles
- Accesos a información sensible (credenciales de dispositivos, datos privados)
- Ejecución de automatizaciones

Lo que NO requiere auditoría:
- Operaciones de solo lectura (queries, listados, búsquedas)
- Operaciones internas de sistema sin actor humano (jobs de Hangfire automáticos sin disparador de usuario)
- Actualizaciones de campos de sesión (`LastSeenAt`)

---

## ARQUITECTURA DE AUDITORÍA

### Flujo obligatorio

```
Application Layer (Command Handler)
  ↓ publica
AuditableActionEvent (Domain Event)
  ↓ consume
Audit Module Handler
  ↓ persiste
AuditEntry (inmutable)
```

**NUNCA** llamar al repositorio de auditoría directamente desde el Command Handler.
La auditoría siempre se dispara via Domain Event.

---

### Entidad AuditEntry (ya definida en DomainModel)

```csharp
// Kamirion.RepairShop.Audit.Domain
public class AuditEntry : Entity<string>
{
    public string TenantId { get; private set; }
    public string EntityType { get; private set; }    // Ej: "RepairTicket"
    public string EntityId { get; private set; }
    public string Action { get; private set; }        // Ej: "StatusChanged"
    public string? BeforeData { get; private set; }   // JSON snapshot anterior
    public string? AfterData { get; private set; }    // JSON snapshot nuevo
    public string PerformedBy { get; private set; }   // UserId
    public string? PerformedByName { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public DateTime PerformedAt { get; private set; }

    // AuditEntry es INMUTABLE: sin setters públicos, sin métodos de modificación
}
```

---

### IAuditService — interfaz en Contracts

```csharp
// Kamirion.RepairShop.Audit.Contracts
public interface IAuditService
{
    Task RecordAsync(
        string tenantId,
        string entityType,
        string entityId,
        string action,
        object? before,
        object? after,
        CancellationToken cancellationToken = default);
}
```

---

### ICurrentUserContext — interfaz en Shared

```csharp
// Kamirion.RepairShop.Shared
public interface ICurrentUserContext
{
    string UserId { get; }
    string? UserName { get; }
    string? IpAddress { get; }
    string? UserAgent { get; }
    bool IsAuthenticated { get; }
}
```

Implementar en Infrastructure (Web) leyendo desde `IHttpContextAccessor`.

---

## PATRÓN DE IMPLEMENTACIÓN EN COMMAND HANDLERS

### Patrón estándar — via Domain Event (preferido)

```csharp
public class ChangeRepairTicketStatusCommandHandler
{
    public async Task<Result> Handle(ChangeRepairTicketStatusCommand command, CancellationToken ct)
    {
        var ticket = await _repository.GetByIdAsync(command.TicketId, ct);
        if (ticket is null) return Result.Failure(Error.NotFound("RepairTicket"));

        var before = ticket.Status; // snapshot antes

        var result = ticket.ChangeStatus(command.NewStatus); // lógica de dominio
        if (result.IsFailure) return result;

        // El aggregate internamente hace RaiseDomainEvent(new RepairTicketStatusChangedEvent(...))
        // que incluye before/after snapshot

        await _repository.SaveAsync(ticket, ct);
        return Result.Success();
    }
}
```

```csharp
// Handler en Audit Module
public class RepairTicketStatusChangedAuditHandler : IEventHandler<RepairTicketStatusChangedEvent>
{
    public async Task Handle(RepairTicketStatusChangedEvent @event, CancellationToken ct)
    {
        await _auditService.RecordAsync(
            tenantId: @event.TenantId,
            entityType: "RepairTicket",
            entityId: @event.TicketId,
            action: "StatusChanged",
            before: new { Status = @event.PreviousStatus },
            after: new { Status = @event.NewStatus },
            ct);
    }
}
```

### Patrón alternativo — via IAuditService directo (solo si no hay Domain Event)

Usar únicamente cuando la operación no genera un Domain Event propio y el costo de crear uno no está justificado:

```csharp
public async Task<Result> Handle(UpdateCustomerNotesCommand command, CancellationToken ct)
{
    var customer = await _repository.GetByIdAsync(command.CustomerId, ct);

    var before = new { customer.Notes };
    customer.UpdateNotes(command.Notes);
    var after = new { customer.Notes };

    await _repository.SaveAsync(customer, ct);

    // Auditoría directa solo como excepción documentada
    await _auditService.RecordAsync(
        _tenantContext.TenantId, "Customer", customer.Id,
        "NotesUpdated", before, after, ct);

    return Result.Success();
}
```

---

## ACCIONES AUDITABLES — REFERENCIA RÁPIDA

| Módulo | Entidad | Acciones a auditar |
|---|---|---|
| RepairWorkflow | RepairTicket | Created, StatusChanged, Assigned, Unassigned, Completed, Cancelled |
| RepairWorkflow | Estimate | Requested, Approved, Rejected |
| Customers | Customer | Created, Updated, Blocked, Unblocked, Deleted |
| Inventory | InventoryItem | Created, Updated, Deleted |
| Inventory | StockMovement | Inbound, Outbound, Adjustment, Reservation, Consumption |
| Payments | Payment | Registered, Refunded, Failed |
| Identity | ApplicationUser | Created, RoleChanged, Deactivated, PasswordChanged |
| Identity | Role | Created, PermissionsChanged, Deleted |
| Configuration | Settings | Changed (con before/after) |

---

## SNAPSHOTS — REGLAS DE SERIALIZACIÓN

```csharp
// CORRECTO: snapshot acotado, solo campos relevantes
before: new { ticket.Status, ticket.AssignedTechnicianId }

// INCORRECTO: serializar el agregado completo (demasiado volumen)
before: ticket // PROHIBIDO
```

- Usar objetos anónimos acotados a los campos que cambian
- Serializar a JSON con `System.Text.Json`
- No incluir datos sensibles en snapshots (contraseñas, tokens, credenciales)
- Máximo ~20 campos por snapshot

---

## CHECKLIST AL GENERAR CÓDIGO CON OPERACIONES DE ESCRITURA

Antes de dar la tarea por finalizada, verificar:

- [ ] Toda operación de escritura tiene su correspondiente registro de auditoría
- [ ] La auditoría se dispara via Domain Event siempre que sea posible
- [ ] `AuditEntry` no tiene métodos de modificación (es inmutable)
- [ ] Los snapshots before/after son objetos acotados, no el agregado completo
- [ ] `ICurrentUserContext` está inyectado y el `UserId` está siendo capturado
- [ ] No se llama a `IAuditService` desde Domain Layer (solo desde Application o via eventos)
- [ ] Los datos sensibles no están incluidos en los snapshots
