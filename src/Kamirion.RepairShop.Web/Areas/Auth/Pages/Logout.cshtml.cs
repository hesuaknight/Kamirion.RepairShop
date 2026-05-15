using Kamirion.RepairShop.Identity.Application.Commands.Logout;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kamirion.RepairShop.Web.Areas.Auth.Pages;

[AllowAnonymous]
public class LogoutModel(ISender mediator) : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        await mediator.Send(new LogoutCommand());
        return LocalRedirect("/auth/login");
    }
}
