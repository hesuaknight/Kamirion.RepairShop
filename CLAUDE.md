# CLAUDE.md — Kamirion.RepairShop

## IDENTIDAD DEL PROYECTO
- Solución: `Kamirion.RepairShop`
- Stack: ASP.NET Core 10 + EF Core + SQL Server + Hangfire + Serilog
- Arquitectura: Modular Monolith + Clean Architecture
- Naming: `Kamirion.RepairShop.{Module}.{Layer}`

---

## REGLAS GLOBALES ACTIVAS SIEMPRE

### PROHIBIDO
- Strings de UI hardcodeados en código C# o Razor (ni en controllers, ni en services, ni en views)
- Lógica de negocio en la capa Presentation
- Referencias directas entre módulos (solo via Contracts o eventos)
- Acceso directo a DB desde Application o Domain
- Guardar archivos en filesystem local del contenedor

### OBLIGATORIO en todo código generado
- Nullable reference types habilitado (`#nullable enable` si el archivo lo requiere)
- Manejo de errores via `Result<T>` / `Error` del Shared Kernel
- Inyección de dependencias por interfaz, nunca por implementación concreta
- `async/await` en toda operación I/O
- Nombres en inglés para código; comentarios pueden ser en español

---

## SKILLS ESPECIALIZADOS

Cuando el código generado involucre alguna de estas áreas, leer el skill correspondiente ANTES de escribir código:

| Área | Skill | Activar cuando... |
|---|---|---|
| Textos de UI / mensajes | `.claude/rules/localization.md` | Se escribe cualquier string visible al usuario |
| Acciones de usuario | `.claude/rules/audit.md` | Se modifica estado, se crea o elimina cualquier entidad |
| Seguridad | `.claude/rules/security.md` | Al finalizar CUALQUIER tarea con código nuevo |
| Tests unitarios | `.claude/rules/unit-testing.md` | Se solicitan o generan tests |
| UI / Vistas Razor | `.claude/rules/design-system.md` | Se genera o modifica cualquier archivo `.cshtml`, `_Layout`, CSS o JS de frontend |
---

## ESTRUCTURA DE MÓDULOS

```
src/
  Kamirion.RepairShop.Web/
  Kamirion.RepairShop.Shared/
  Kamirion.RepairShop.Infrastructure/
  Modules/
    Kamirion.RepairShop.{Module}/
      Kamirion.RepairShop.{Module}.Domain/
      Kamirion.RepairShop.{Module}.Application/
      Kamirion.RepairShop.{Module}.Infrastructure/
      Kamirion.RepairShop.{Module}.Contracts/
tests/
  Kamirion.RepairShop.Architecture.Tests/
  Kamirion.RepairShop.Shared.Tests/
```

---

## FLUJO DE TRABAJO ESPERADO

1. Leer tarea completa antes de escribir código
2. Identificar qué skills aplican
3. Leer los skills relevantes
4. Implementar siguiendo las reglas
5. Ejecutar análisis de seguridad (skill security.md) al finalizar
6. Verificar que `dotnet build` pasa sin errores
7. Verificar que `dotnet test` pasa sin errores nuevos

---

## CONVENCIONES DE CÓDIGO

- Entidades: heredan de `Entity<string>` o `AggregateRoot<string>` con ULID como Id
- IDs: siempre `string` generado con `UlidGenerator.New()`
- Soft delete: entidades operacionales implementan `ISoftDelete`
- Auditable: entidades operacionales implementan `IAuditableEntity`
- Tenant-owned: entidades de negocio implementan `ITenantOwned`
- DTOs: sufijo `Request` para entrada, `Response` para salida
- Commands: sufijo `Command`, handlers sufijo `CommandHandler`
- Queries: sufijo `Query`, handlers sufijo `QueryHandler`
