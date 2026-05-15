using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kamirion.RepairShop.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
