using Kamirion.RepairShop.Identity.Application.Commands.Login;
using Kamirion.RepairShop.Web.Modules.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Kamirion.RepairShop.Web.Areas.Auth.Pages;

[AllowAnonymous]
public class LoginModel(ISender mediator, IStringLocalizer<IdentityResources> localizer) : PageModel
{
    [BindProperty]
    public LoginInputModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return LocalRedirect(ReturnUrl ?? "/");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await mediator.Send(new LoginCommand(Input.Email, Input.Password));

        if (result.IsSuccess)
            return LocalRedirect(ReturnUrl ?? "/");

        var errorKey = result.Error.Code.StartsWith("Validation.")
            ? result.Error.Description
            : "Login_InvalidCredentials_Error";

        ModelState.AddModelError(string.Empty, localizer[errorKey]);
        return Page();
    }
}

public class LoginInputModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
