# Design System — Kamirion.RepairShop
# Versión 1.0 · Mayo 2026

> Documento de referencia para generación de vistas Razor Pages/MVC.
> Claude Code debe consultar este documento antes de generar cualquier vista HTML/Razor.

---

## 1. Identidad Visual

### Concepto
**Dark operational dashboard** — inspirado en Binance Dark Theme y herramientas operacionales de alto tráfico.
El sistema transmite control, velocidad y precisión. No es un CRM amigable ni un e-commerce: es una herramienta de trabajo.

### Principios visuales
- **Dark-first**: fondo base `#0d1117` (GitHub dark). Sin modo claro.
- **Emerald como primario**: único color de acento vibrante. El resto del sistema es neutro.
- **Densidad operacional**: información compacta, no espaciado generoso. Los técnicos necesitan ver más datos en menos scroll.
- **Realtime-friendly**: estados con dots pulsantes, tablas actualizables sin flash.
- **Sin decoración**: cero gradientes, cero sombras dramáticas, cero ilustraciones. Solo datos.

---

## 2. Tipografía

### Fuentes
```html
<!-- En _Layout.cshtml <head> -->
<link href="https://fonts.googleapis.com/css2?family=DM+Sans:wght@400;500;600&family=JetBrains+Mono:wght@400&display=swap" rel="stylesheet">
```

| Rol | Fuente | Peso | Tamaño |
|---|---|---|---|
| Cuerpo general | DM Sans | 400 | 13px |
| Labels, botones | DM Sans | 500 | 12-13px |
| Títulos de sección | DM Sans | 600 | 16-20px |
| Display / page title | DM Sans | 600 | 24-28px |
| IDs, ULIDs, código | JetBrains Mono | 400 | 11-12px |
| Column headers | DM Sans | 500 uppercase | 10px / 0.1em tracking |

### Escala
```css
--font-display:  28px / 600  /* títulos de página */
--font-h1:       20px / 600  /* headings de sección */
--font-h2:       16px / 500  /* sub-sección, modal title */
--font-h3:       13px / 500  /* card title */
--font-body:     13px / 400  /* cuerpo general */
--font-caption:  11px / 400  /* metadata, timestamps */
--font-label:    10px / 500 uppercase  /* column headers, section labels */
--font-mono:     12px / 400  /* IDs, ULID, tokens */
```

---

## 3. Paleta de Colores

### Variables CSS — archivo `wwwroot/css/variables.css`

```css
:root {
  /* === BACKGROUNDS === */
  --rs-bg-base:     #0d1117;   /* fondo de página */
  --rs-bg-surface:  #161b22;   /* sidebar, topbar, panels */
  --rs-bg-elevated: #1c2330;   /* cards, stat boxes */
  --rs-bg-overlay:  #21262d;   /* dropdowns, tooltips */

  /* === BORDERS === */
  --rs-border:      rgba(255,255,255,0.08);   /* bordes por defecto */
  --rs-border-md:   rgba(255,255,255,0.14);   /* hover, inputs */
  --rs-border-lg:   rgba(255,255,255,0.22);   /* focus, activo */

  /* === TEXTO === */
  --rs-text-primary:   #e6edf3;  /* texto principal */
  --rs-text-secondary: #8b949e;  /* texto secundario, labels */
  --rs-text-muted:     #484f58;  /* texto deshabilitado, placeholders */

  /* === EMERALD (primario) === */
  --rs-emerald:        #10b981;
  --rs-emerald-light:  #34d399;
  --rs-emerald-dim:    #059669;
  --rs-emerald-bg:     rgba(16,185,129,0.10);
  --rs-emerald-border: rgba(16,185,129,0.25);

  /* === SEMÁNTICOS === */
  --rs-sky:        #38bdf8;  --rs-sky-bg:    rgba(56,189,248,0.10);
  --rs-amber:      #f59e0b;  --rs-amber-bg:  rgba(245,158,11,0.12);
  --rs-red:        #f87171;  --rs-red-bg:    rgba(248,113,113,0.12);
  --rs-violet:     #a78bfa;  --rs-violet-bg: rgba(167,139,250,0.10);
  --rs-slate:      #64748b;  --rs-slate-bg:  rgba(100,116,139,0.15);
  --rs-orange:     #fb923c;  --rs-orange-bg: rgba(251,146,60,0.12);

  /* === RADIOS === */
  --rs-radius-sm: 4px;
  --rs-radius-md: 6px;
  --rs-radius-lg: 10px;
  --rs-radius-xl: 14px;
  --rs-radius-pill: 999px;

  /* === TIPOGRAFÍA === */
  --rs-font: 'DM Sans', system-ui, sans-serif;
  --rs-font-mono: 'JetBrains Mono', monospace;
}
```

### Override de Bootstrap
```css
/* En variables.css — después de las variables propias */
:root {
  --bs-primary:          #10b981;
  --bs-primary-rgb:      16,185,129;
  --bs-body-bg:          #0d1117;
  --bs-body-color:       #e6edf3;
  --bs-card-bg:          #161b22;
  --bs-card-border-color: rgba(255,255,255,0.08);
  --bs-border-color:     rgba(255,255,255,0.08);
  --bs-secondary-bg:     #161b22;
  --bs-tertiary-bg:      #1c2330;
  --bs-body-font-family: 'DM Sans', system-ui, sans-serif;
}
```

---

## 4. Espaciado

Sistema base de **4px**. Todos los valores de margin, padding y gap son múltiplos de 4.

| Variable | Valor | Uso típico |
|---|---|---|
| `--space-1` | 4px | gap mínimo, padding de icon |
| `--space-2` | 8px | gap inline, padding badge |
| `--space-3` | 12px | padding input, gap entre campos |
| `--space-4` | 16px | padding card, gap entre rows |
| `--space-5` | 20px | separación entre grupos |
| `--space-6` | 24px | separación entre secciones |
| `--space-8` | 32px | padding de página |
| `--space-12` | 48px | separación de bloques mayores |

---

## 5. Componentes

### 5.1 Botones

```html
<!-- Primary — acción principal de la página -->
<button class="btn btn-rs-primary">
  <i class="ti ti-plus"></i> Nuevo ticket
</button>

<!-- Secondary — acciones de apoyo -->
<button class="btn btn-rs-secondary">
  <i class="ti ti-filter"></i> Filtrar
</button>

<!-- Ghost — acciones en tablas y contextos densos -->
<button class="btn btn-rs-ghost">
  <i class="ti ti-eye"></i> Ver
</button>

<!-- Danger — destructivas, siempre con confirmación -->
<button class="btn btn-rs-danger">
  <i class="ti ti-trash"></i> Eliminar
</button>
```

```css
/* Tamaños */
.btn-sm { height: 26px; padding: 0 10px; font-size: 11px; }
.btn    { height: 32px; padding: 0 14px; font-size: 12px; }  /* default */
.btn-lg { height: 38px; padding: 0 20px; font-size: 13px; }

/* Icon-only */
.btn-icon { width: 32px; padding: 0; justify-content: center; }
```

**Regla:** Una sola acción primary por vista. Si hay dos acciones importantes, una es secondary.

### 5.2 Badges de Estado (WorkflowState)

Mapping obligatorio entre enum del dominio y clase CSS:

| WorkflowState (C#) | CSS class | Color |
|---|---|---|
| `Pending` | `badge-rs-pending` | Slate (neutro) |
| `Received` | `badge-rs-received` | Sky (azul) |
| `InDiagnosis` | `badge-rs-diagnosis` | Violet (púrpura) |
| `WaitingParts` | `badge-rs-waiting` | Amber (amarillo) |
| `InRepair` | `badge-rs-repair` + `badge-pulse` | Emerald pulsante |
| `QualityCheck` | `badge-rs-qa` | Sky claro |
| `ReadyForPickup` | `badge-rs-ready` | Emerald suave |
| `Delivered` | `badge-rs-delivered` | Slate (neutro) |
| `Cancelled` | `badge-rs-cancelled` | Red |

```html
<!-- Uso en Razor -->
<span class="badge-rs badge-rs-@Model.State.ToString().ToLower()">
  @Localizer[Model.State.ToString()]
</span>
```

**Importante:** `InRepair` siempre usa el dot pulsante para indicar actividad en curso.

### 5.3 Badges de Prioridad

```html
<span class="priority-rs priority-@Model.Priority.ToString().ToLower()">
  <i class="ti ti-@GetPriorityIcon(Model.Priority)"></i>
  @Localizer[Model.Priority.ToString()]
</span>
```

| Priority | Icono | Color |
|---|---|---|
| Low | `ti-arrow-down` | Muted |
| Medium | `ti-arrow-right` | Amber |
| High | `ti-arrow-up` | Orange |
| Critical | `ti-flame` | Red |

### 5.4 Inputs y Formularios

```html
<!-- Input estándar -->
<div class="rs-field">
  <label class="rs-label">@Localizer["Label.CustomerName"]</label>
  <input class="rs-input" type="text" asp-for="CustomerName" />
  <span class="rs-field-error" asp-validation-for="CustomerName"></span>
</div>

<!-- Select -->
<div class="rs-field">
  <label class="rs-label">@Localizer["Label.AssignedTech"]</label>
  <select class="rs-select" asp-for="TechnicianId" asp-items="Model.Technicians">
    <option value="">@Localizer["Select.Unassigned"]</option>
  </select>
</div>

<!-- Búsqueda global en topbar -->
<div class="rs-search">
  <i class="ti ti-search" aria-hidden="true"></i>
  <input type="text" placeholder="@Localizer["Search.Placeholder"]" id="global-search" />
</div>
```

**Focus ring:** Todos los inputs y selects muestran `box-shadow: 0 0 0 3px var(--rs-emerald-bg)` y `border-color: var(--rs-emerald)` en `:focus`.

### 5.5 Cards

Tres variantes:

```html
<!-- Stat card — métricas del dashboard -->
<div class="rs-stat-card">
  <div class="rs-stat-header">
    <span class="rs-stat-label">@Localizer["Stat.ActiveTickets"]</span>
    <div class="rs-stat-icon rs-icon-emerald">
      <i class="ti ti-tools" aria-hidden="true"></i>
    </div>
  </div>
  <div class="rs-stat-value">47</div>
  <div class="rs-stat-change rs-change-up">
    <i class="ti ti-arrow-up"></i> +8 @Localizer["Change.ThisWeek"]
  </div>
</div>

<!-- Info card — entidad (ticket, cliente, dispositivo) -->
<div class="rs-card">
  <div class="rs-card-header">
    <div>
      <h3 class="rs-card-title">iPhone 14 Pro Max</h3>
      <p class="rs-card-sub">TKT-01HZZX · Lucía Fernández</p>
    </div>
    <span class="badge-rs badge-rs-repair badge-pulse">@Localizer["Status.InRepair"]</span>
  </div>
  <div class="rs-card-body"><!-- contenido --></div>
  <div class="rs-card-footer"><!-- acciones --></div>
</div>

<!-- Panel — agrupación de contenido -->
<div class="rs-panel">
  <div class="rs-panel-header">
    <h2 class="rs-panel-title">@Localizer["Panel.RecentTickets"]</h2>
    <a href="@Url.Page("/Tickets/Index")" class="btn btn-rs-ghost btn-sm">@Localizer["Action.ViewAll"]</a>
  </div>
  <div class="rs-panel-body"><!-- contenido --></div>
</div>
```

### 5.6 Alertas

```html
<!-- info | success | warning | danger -->
<div class="rs-alert rs-alert-warning" role="alert">
  <i class="ti ti-alert-triangle" aria-hidden="true"></i>
  <div>
    <p class="rs-alert-title">@Localizer["Alert.SlaAtRisk.Title"]</p>
    <p class="rs-alert-body">@Localizer["Alert.SlaAtRisk.Body"]</p>
  </div>
</div>
```

**Regla:** Usar `border-left: 3px solid` como indicador semántico. Sin bordes completos de color.

### 5.7 Timeline de Ticket

```html
<div class="rs-timeline" aria-label="@Localizer["Timeline.Aria"]">
  @foreach (var evt in Model.Timeline)
  {
    <div class="rs-tl-item">
      <div class="rs-tl-indicator">
        <div class="rs-tl-dot @(evt.IsLatest ? "rs-tl-dot--active rs-tl-dot--pulse" : "")"></div>
        @if (!evt.IsLast) { <div class="rs-tl-line"></div> }
      </div>
      <div class="rs-tl-content">
        <p class="rs-tl-event">@Localizer[evt.EventKey]</p>
        <p class="rs-tl-meta">@evt.OccurredAt.ToString("dd MMM yyyy, HH:mm") · @evt.ActorName</p>
      </div>
    </div>
  }
</div>
```

---

## 6. Layout — App Shell

### Estructura HTML de `_Layout.cshtml`

```
┌─────────────────────────────────────────────────────┐
│  TOPBAR (48px)                                      │
│  Logo · TenantName · BranchSelector │ Search·Bell·Avatar │
├──────┬──────────────────────────────────────────────┤
│      │                                              │
│  S   │  MAIN CONTENT AREA                          │
│  I   │  padding: 24px 32px (desktop)               │
│  D   │  padding: 16px     (mobile)                 │
│  E   │                                              │
│  B   │                                              │
│  A   │                                              │
│  R   │                                              │
│  52px│                                              │
└──────┴──────────────────────────────────────────────┘
```

### Topbar
- Height: **48px** fija
- Left: `⬡` logo · `TenantName` (bold) · `BranchSelector` (dropdown compacto)
- Right: `GlobalSearch` · `NotificationBell` (con dot rojo si hay notifs) · `UserAvatar` (iniciales)
- Background: `var(--rs-bg-surface)`

### Sidebar
- Width desktop: **52px** (collapsed, solo íconos + tooltips)
- Width desktop expandido (≥1280px): **220px** opcional — togglable
- Mobile: se convierte en **bottom navigation bar** (5 ítems máximo)
- Ítems activos: `color: var(--rs-emerald)` + `background: var(--rs-emerald-bg)` + `border-left: 2px solid var(--rs-emerald)`

### Navegación principal (orden)
1. `ti-layout-dashboard` — Dashboard
2. `ti-tools` — Tickets
3. `ti-users` — Clientes
4. `ti-package` — Inventario
5. `ti-currency-dollar` — Pagos
6. *(separador)*
7. `ti-settings` — Configuración (al fondo)

### Breadcrumb
Solo en vistas de **detalle** (un ticket, un cliente, un dispositivo). Nunca en listados ni dashboard.

```html
<nav class="rs-breadcrumb" aria-label="@Localizer["Nav.Breadcrumb"]">
  <a href="/tickets">@Localizer["Nav.Tickets"]</a>
  <i class="ti ti-chevron-right" aria-hidden="true"></i>
  <span>@Model.TicketCode</span>
</nav>
```

---

## 7. Tabla de Tickets

Componente central del sistema. Columnas según breakpoint:

| Breakpoint | Columnas visibles |
|---|---|
| Desktop ≥992px | ID · Dispositivo/Cliente · Estado · Prioridad · Técnico · Ingreso · Acciones |
| Tablet 768-991px | ID · Dispositivo · Estado · Técnico · Acciones |
| Mobile <768px | Cards stacked (sin tabla) |

```html
<table class="rs-table" id="tickets-table">
  <thead>
    <tr>
      <th>@Localizer["Col.TicketId"]</th>
      <th>@Localizer["Col.Device"]</th>
      <th>@Localizer["Col.Status"]</th>
      <th class="d-none d-md-table-cell">@Localizer["Col.Priority"]</th>
      <th class="d-none d-lg-table-cell">@Localizer["Col.Technician"]</th>
      <th class="d-none d-lg-table-cell">@Localizer["Col.ReceivedAt"]</th>
      <th></th>
    </tr>
  </thead>
  <tbody>
    @foreach (var ticket in Model.Tickets)
    {
      <tr class="rs-table-row" data-href="@Url.Page("/Tickets/Detail", new { id = ticket.Id })">
        <td><span class="rs-ticket-id">@ticket.Code</span></td>
        <td>
          <p class="rs-device-name">@ticket.DeviceName</p>
          <p class="rs-customer-name">@ticket.CustomerName</p>
        </td>
        <td>
          <span class="badge-rs badge-rs-@ticket.State.ToString().ToLower() @(ticket.State == WorkflowState.InRepair ? "badge-pulse" : "")">
            @Localizer[ticket.State.ToString()]
          </span>
        </td>
        <td class="d-none d-md-table-cell">
          <span class="priority-rs priority-@ticket.Priority.ToString().ToLower()">
            @Localizer[ticket.Priority.ToString()]
          </span>
        </td>
        <td class="d-none d-lg-table-cell">
          <span class="rs-tech-avatar" title="@ticket.TechnicianName">@ticket.TechnicianInitials</span>
        </td>
        <td class="d-none d-lg-table-cell rs-date-cell">@ticket.ReceivedAt.ToString("dd MMM")</td>
        <td>
          <button class="btn btn-rs-ghost btn-icon btn-sm" aria-label="@Localizer["Action.MoreOptions"]">
            <i class="ti ti-dots-vertical" aria-hidden="true"></i>
          </button>
        </td>
      </tr>
    }
  </tbody>
</table>
```

**Filas clickeables:** `data-href` con JS simple en `site.js`:
```javascript
document.querySelectorAll('.rs-table-row[data-href]').forEach(row => {
  row.addEventListener('click', () => { window.location = row.dataset.href; });
  row.style.cursor = 'pointer';
});
```

---

## 8. Íconos

**Librería:** [Tabler Icons](https://tabler.io/icons) — Webfont outline.

```html
<!-- En _Layout.cshtml -->
<link href="https://cdn.jsdelivr.net/npm/@tabler/icons-webfont@3.34.0/dist/tabler-icons.min.css" rel="stylesheet">
```

Uso: `<i class="ti ti-{nombre}" aria-hidden="true"></i>`

Íconos frecuentes en el dominio:

| Entidad / Acción | Icono |
|---|---|
| Dashboard | `ti-layout-dashboard` |
| Ticket / Reparación | `ti-tools` |
| Cliente | `ti-user` |
| Dispositivo | `ti-device-mobile` |
| Inventario | `ti-package` |
| Pago | `ti-currency-dollar` |
| Notificación | `ti-bell` |
| WhatsApp | `ti-brand-whatsapp` |
| Configuración | `ti-settings` |
| Nuevo / Agregar | `ti-plus` |
| Eliminar | `ti-trash` |
| Editar | `ti-pencil` |
| Ver detalle | `ti-eye` |
| Avanzar estado | `ti-arrow-right` |
| Historial / Timeline | `ti-timeline` |
| SLA / Tiempo | `ti-clock` |
| Técnico | `ti-user-bolt` |
| Búsqueda | `ti-search` |
| Filtro | `ti-filter` |
| Exportar | `ti-download` |

**Regla accesibilidad:** Íconos decorativos → `aria-hidden="true"`. Íconos como único indicador (botón icon-only) → `aria-label` en el botón.

---

## 9. Realtime (SignalR)

Los componentes con actualización en tiempo real siguen estas convenciones:

- **Dot pulsante:** badge con clase `badge-pulse` → indica estado activo en curso (`InRepair`)
- **Actualización de contador:** stat cards con `id="stat-active-tickets"` se actualizan via JS sin reload
- **Toast de actualización:** cuando un ticket cambia de estado mientras el usuario está en el listado, mostrar toast non-blocking en esquina inferior derecha

```html
<!-- Toast de actualización realtime -->
<div class="rs-toast rs-toast-info" role="status" aria-live="polite">
  <i class="ti ti-refresh" aria-hidden="true"></i>
  <span>@Localizer["Realtime.TicketUpdated", ticketCode]</span>
</div>
```

---

## 10. Portal del Cliente

El portal usa un subset simplificado del design system. Diferencias:

- Mismo dark theme, mismo emerald
- Sin sidebar — layout centrado, max-width 480px
- Sin topbar completo — solo logo del tenant + nombre del negocio
- Tipografía ligeramente más grande (body: 14px) para facilitar lectura en mobile
- Sin tabla de tickets — solo cards stacked con timeline
- CTAs grandes (44px height) para facilidad táctil

---

## 11. Accesibilidad

- Contraste mínimo AA en todo texto sobre fondo oscuro
- Todos los inputs con `<label>` asociado (atributo `for` o `asp-for`)
- Íconos decorativos con `aria-hidden="true"`
- Botones icon-only con `aria-label`
- Roles ARIA en timeline (`role="list"`, `role="listitem"`) y alertas (`role="alert"`)
- Focus visible en todos los elementos interactivos (ring esmeralda)

---

## 12. Archivos CSS — Estructura sugerida

```
wwwroot/
  css/
    variables.css        ← tokens, paleta, overrides Bootstrap
    layout.css           ← topbar, sidebar, shell
    components.css       ← buttons, badges, cards, forms, table, timeline, alerts
    utilities.css        ← clases de ayuda (rs-text-mono, rs-date-cell, etc.)
    animations.css       ← badge-pulse, toast, skeleton loaders
  js/
    site.js              ← clickable rows, topbar interactivity
    realtime.js          ← SignalR hub connections
```

Bootstrap se carga primero, luego `variables.css` override las custom properties.

---

## 13. Reglas para Claude Code

Al generar vistas Razor Pages/MVC de cualquier módulo:

1. **Nunca** usar colores o strings hardcodeados — ver `localization.md` y este documento
2. **Siempre** usar las clases `rs-*` definidas en este documento para componentes
3. **Siempre** usar `@Localizer["clave"]` para texto visible al usuario
4. Íconos: solo Tabler Icons webfont, solo outline, nunca SVG inline excepto logo
5. Nuevos badges de estado → respetar la tabla WorkflowState → CSS class del punto 5.2
6. Formularios → campos con `rs-field` wrapper + `rs-label` + `rs-input` + validación con `rs-field-error`
7. Tablas → usar `rs-table` con columnas responsivas según punto 7
8. El layout shell (`_Layout.cshtml`) no se modifica por features — es infraestructura
