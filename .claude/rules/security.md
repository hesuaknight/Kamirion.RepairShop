# SKILL: Análisis de Ciberseguridad
# Archivo: .claude/rules/security.md
# Activar cuando: AL FINALIZAR cualquier tarea que genere código nuevo o modifique código existente

---

## REGLA PRINCIPAL

**Al finalizar toda tarea con código nuevo, ejecutar este análisis de seguridad antes de dar la tarea por completada.**

El análisis debe producir un reporte mínimo al final de la respuesta con el siguiente formato:

```
## Security Review — {ARCH-XXX o nombre de tarea}
Riesgo general: BAJO / MEDIO / ALTO
Hallazgos: {N}
{lista de hallazgos o "Sin hallazgos detectados"}
```

---

## CATEGORÍAS DE ANÁLISIS

Evaluar el código generado contra las siguientes categorías. Solo reportar categorías con hallazgos o marcadas explícitamente como verificadas.

---

### CAT-01 — Inyección (SQL, Command, LDAP)

**Verificar:**
- ¿Se construyen queries SQL con concatenación de strings? → CRÍTICO
- ¿Se usa EF Core con parámetros? → OK
- ¿Se interpolan valores de usuario en queries raw? → CRÍTICO
- ¿Se usan `FromSqlRaw` o `ExecuteSqlRaw` con input de usuario sin parametrizar? → ALTO

**Patrón seguro:**
```csharp
// CORRECTO
context.Tickets.Where(t => t.TicketNumber == input).ToListAsync();
context.Database.ExecuteSqlRaw("SELECT * FROM Tickets WHERE Id = {0}", id);

// INCORRECTO — CRÍTICO
context.Database.ExecuteSqlRaw($"SELECT * FROM Tickets WHERE Id = '{id}'");
```

---

### CAT-02 — Autenticación y Autorización

**Verificar:**
- ¿Los endpoints nuevos tienen `[Authorize]`? Si no tienen, ¿es intencional y está documentado?
- ¿Se verifica que el recurso accedido pertenece al tenant del usuario autenticado?
- ¿Se verifica que el recurso accedido pertenece al usuario correcto (no solo que está autenticado)?
- ¿Los endpoints del Customer Portal tienen autenticación separada de la app interna?
- ¿El Hangfire Dashboard está protegido en producción?

**Patrón obligatorio en controllers:**
```csharp
// Todo controller operacional debe tener:
[Authorize]
[RequireTenant] // custom filter que verifica tenant resuelto
public class RepairTicketsController : Controller { }
```

---

### CAT-03 — Tenant Isolation (CRÍTICO para SaaS)

**Verificar:**
- ¿Alguna query usa `IgnoreQueryFilters()` sin justificación documentada?
- ¿Se recibe un `tenantId` como parámetro de entrada desde el usuario? → SIEMPRE debe tomarse del `ITenantContext`, nunca del input
- ¿Alguna operación podría exponer datos de otro tenant?
- ¿Los IDs recibidos del cliente son validados contra el tenant actual?

**Patrón obligatorio:**
```csharp
// CORRECTO: TenantId siempre del contexto autenticado
var tenantId = _tenantContext.TenantId; // del middleware

// INCORRECTO — CRÍTICO
var tenantId = command.TenantId; // del input del usuario
```

---

### CAT-04 — Exposición de Datos Sensibles

**Verificar:**
- ¿Los DTOs de respuesta exponen campos innecesarios? (IDs internos, hashes, datos de otros tenants)
- ¿Se loguean datos sensibles con Serilog? (contraseñas, tokens, credenciales de dispositivos, IMEI)
- ¿Los mensajes de error devueltos al cliente exponen stack traces o detalles internos?
- ¿Los snapshots de auditoría contienen contraseñas o tokens?
- ¿Las connection strings u otros secretos están hardcodeados?

**Patrón seguro:**
```csharp
// Errores al cliente: mensaje genérico
return Problem("An error occurred processing your request.");

// Logs internos: detalle completo pero sin datos sensibles
_logger.LogError(ex, "Error processing ticket {TicketId}", ticketId);
// NO: _logger.LogError("Password was {Password}", user.Password);
```

---

### CAT-05 — CSRF y Request Forgery

**Verificar:**
- ¿Los forms POST en Razor Pages tienen `@Html.AntiForgeryToken()` o `asp-antiforgery="true"`?
- ¿Los controllers que reciben POST/PUT/DELETE tienen `[ValidateAntiForgeryToken]`?
- ¿Los endpoints de API (si los hay) usan autenticación por token en vez de cookies?

---

### CAT-06 — Validación de Input

**Verificar:**
- ¿Todo input de usuario es validado antes de llegar al Application Layer?
- ¿Se validan rangos, longitudes máximas, formatos?
- ¿Se sanitiza input que se va a renderizar como HTML?
- ¿Los uploads de archivos validan extensión Y content-type Y tamaño máximo?

**Patrón obligatorio para uploads:**
```csharp
var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
var allowedContentTypes = new[] { "image/jpeg", "image/png", "application/pdf" };
if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) // validar
if (!allowedContentTypes.Contains(file.ContentType)) // validar
if (file.Length > 10 * 1024 * 1024) // máximo 10MB
```

---

### CAT-07 — Mass Assignment

**Verificar:**
- ¿Los comandos/DTOs de entrada exponen solo los campos que el usuario puede modificar?
- ¿Se mapea directamente un objeto de request a una entidad de dominio sin filtrar?
- ¿El usuario puede sobrescribir `TenantId`, `CreatedAt`, `CreatedBy` u otros campos de sistema?

---

### CAT-08 — Secrets y Configuración

**Verificar:**
- ¿Hay API keys, connection strings o passwords hardcodeados en el código?
- ¿Los secrets están en `appsettings.json` commiteados? (deben estar en variables de entorno o User Secrets)
- ¿Los archivos `.env` están en `.gitignore`?

---

### CAT-09 — Background Jobs (Hangfire)

**Verificar:**
- ¿Los jobs son idempotentes? (ejecutar dos veces el mismo job no genera datos duplicados)
- ¿Los jobs validan que el tenant sigue activo antes de ejecutar?
- ¿Los jobs no reciben datos sensibles en sus argumentos? (Hangfire serializa argumentos en DB)
- ¿El Dashboard de Hangfire está protegido con autenticación en producción?

---

### CAT-10 — Dependencias y Supply Chain

**Verificar (cuando se agregan paquetes NuGet nuevos):**
- ¿El paquete es ampliamente adoptado y mantenido activamente?
- ¿La versión tiene vulnerabilidades conocidas? (verificar en https://osv.dev o NuGet Advisory)
- ¿Se está usando la última versión estable?

---

## ESCALA DE RIESGO

| Nivel | Criterio | Acción requerida |
|---|---|---|
| CRÍTICO | Vulnerabilidad explotable directamente (SQLi, tenant bypass, auth bypass) | Bloquear tarea. No continuar hasta resolver. |
| ALTO | Exposición de datos, validación ausente en input sensible | Resolver en la misma tarea antes de cerrar. |
| MEDIO | Mejora de seguridad recomendada, no explotable directamente | Documentar como deuda técnica de seguridad. |
| BAJO | Observación menor, buenas prácticas | Mencionar, no bloquea. |

---

## FORMATO DE REPORTE OBLIGATORIO

Al finalizar toda tarea, agregar al final de la respuesta:

```
---
## Security Review — {nombre de tarea}
Riesgo general: BAJO
Categorías analizadas: CAT-01, CAT-02, CAT-03, CAT-04, CAT-07, CAT-08
Hallazgos: 0
Sin hallazgos detectados.
```

```
---
## Security Review — {nombre de tarea}
Riesgo general: ALTO
Categorías analizadas: CAT-01, CAT-02, CAT-03, CAT-05, CAT-06
Hallazgos: 2

[ALTO] CAT-06 — El campo `FileUpload` no valida content-type, solo extensión.
  → Agregar validación de content-type antes de procesar el archivo.

[MEDIO] CAT-04 — El DTO `RepairTicketResponse` expone `InternalNotes` que no debería ser visible al cliente del portal.
  → Crear DTO separado para el Customer Portal sin ese campo.
```
