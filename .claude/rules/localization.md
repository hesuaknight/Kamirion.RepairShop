# SKILL: Localización y Resources
# Archivo: .claude/rules/localization.md
# Activar cuando: se escribe cualquier string visible al usuario en UI, validaciones, notificaciones o logs de sistema

---

## REGLA PRINCIPAL

**NINGÚN texto visible al usuario puede estar hardcodeado en el código.**

Esto incluye sin excepción:
- Labels, títulos, botones en vistas Razor
- Mensajes de validación
- Mensajes de error devueltos al usuario
- Notificaciones y alertas
- Mensajes de WhatsApp y Email
- Texto en PDFs generados
- Mensajes de respuesta de API al cliente

Lo que NO aplica (puede ir en código):
- Nombres de variables, propiedades, clases
- Claves de configuración (`appsettings.json`)
- Nombres técnicos de logs internos (Serilog, no visibles al usuario)
- Comentarios de código

---

## ARQUITECTURA DE RESOURCES

### Ubicación de archivos

```
src/Kamirion.RepairShop.Web/
  Resources/
    Shared/
      CommonResources.resx              ← textos globales (botones, acciones comunes)
      CommonResources.es.resx
      CommonResources.en.resx
    Modules/
      Identity/
        IdentityResources.resx
        IdentityResources.es.resx
        IdentityResources.en.resx
      RepairWorkflow/
        RepairWorkflowResources.resx
        RepairWorkflowResources.es.resx
        RepairWorkflowResources.en.resx
      Customers/
        CustomersResources.resx
        CustomersResources.es.resx
        CustomersResources.en.resx
      {Module}/
        {Module}Resources.resx
        {Module}Resources.es.resx
        {Module}Resources.en.resx
    Validation/
      ValidationResources.resx
      ValidationResources.es.resx
      ValidationResources.en.resx
    Notifications/
      NotificationResources.resx
      NotificationResources.es.resx
      NotificationResources.en.resx
```

### Convención de naming de claves

```
{Entidad}_{Acción/Contexto}_{TipoDeMensaje}

Ejemplos:
  RepairTicket_Created_Success
  RepairTicket_NotFound_Error
  Customer_Email_Required_Validation
  Estimate_Approve_Confirm
  Common_Save_Button
  Common_Cancel_Button
  Common_Delete_Confirm
```

---

## CONFIGURACIÓN EN PROGRAM.CS

```csharp
builder.Services.AddLocalization(options =>
    options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "es", "es-AR", "es-CL", "es-MX", "en", "en-US" };
    options.SetDefaultCulture("es")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);

    // Resolución estricta por cultura del navegador
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

// En el pipeline HTTP (debe ir antes de UseRouting):
app.UseRequestLocalization();
```

---

## USO EN VISTAS RAZOR

```csharp
// En el PageModel o Controller, inyectar:
private readonly IStringLocalizer<RepairWorkflowResources> _localizer;

public MyPageModel(IStringLocalizer<RepairWorkflowResources> localizer)
{
    _localizer = localizer;
}
```

```razor
@inject IStringLocalizer<RepairWorkflowResources> Localizer

<!-- CORRECTO -->
<button>@Localizer["RepairTicket_Save_Button"]</button>
<h1>@Localizer["RepairTicket_List_Title"]</h1>

<!-- INCORRECTO — PROHIBIDO -->
<button>Guardar</button>
<h1>Lista de Tickets</h1>
```

---

## USO EN VALIDACIONES (DataAnnotations)

```csharp
// CORRECTO
[Required(ErrorMessageResourceType = typeof(ValidationResources),
          ErrorMessageResourceName = "Customer_Name_Required")]
public string FullName { get; set; } = string.Empty;

// INCORRECTO — PROHIBIDO
[Required(ErrorMessage = "El nombre es requerido")]
public string FullName { get; set; } = string.Empty;
```

---

## USO EN APPLICATION LAYER (errores devueltos al usuario)

Los errores en Application Layer usan claves de resource, NO strings literales:

```csharp
// CORRECTO: la clave del error es un resource key, no un mensaje
public static Error NotFound() =>
    new("RepairTicket.NotFound", "RepairTicket_NotFound_Error");
    //  ^ código técnico          ^ clave de resource

// El Presentation Layer resuelve la clave al texto localizado:
if (result.IsFailure)
    return BadRequest(_localizer[result.Error.Description]);
```

---

## USO EN NOTIFICACIONES (WhatsApp / Email)

Los templates de notificaciones NO deben contener texto hardcodeado.
Deben referenciar templates almacenados en DB por cultura:

```csharp
// CORRECTO
var template = await _templateRepository.GetAsync(
    templateKey: "RepairTicket_StatusChanged",
    culture: tenantContext.DefaultCulture,
    cancellationToken);

// INCORRECTO — PROHIBIDO
var message = $"Tu reparación {ticketNumber} cambió de estado a {status}";
```

---

## CHECKLIST AL GENERAR CÓDIGO CON TEXTOS

Antes de dar la tarea por finalizada, verificar:

- [ ] No existe ningún string en español/inglés hardcodeado en archivos `.cs` o `.cshtml` que sea visible al usuario
- [ ] Cada texto nuevo tiene su clave en el archivo `.resx` correspondiente al módulo
- [ ] El archivo `.resx` base existe (sin cultura) y los archivos de cultura también (`es`, `en` mínimo)
- [ ] Las validaciones usan `ErrorMessageResourceType` y `ErrorMessageResourceName`
- [ ] Los templates de notificación referencian registros en DB, no strings en código
- [ ] `AddLocalization` y `UseRequestLocalization` están configurados en `Program.cs`
