using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kamirion.RepairShop.Web.Areas.Auth.Pages;

[AllowAnonymous]
public class AccessDeniedModel : PageModel
{
    public void OnGet() { }
}
