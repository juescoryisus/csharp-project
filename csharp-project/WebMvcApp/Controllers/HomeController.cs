using Microsoft.AspNetCore.Mvc;

namespace WebMvcApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Error() => View();
}
