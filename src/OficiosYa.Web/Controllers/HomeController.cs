using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OficiosYa.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public IActionResult Index()
    {
        var spa = Path.Combine(_env.WebRootPath, "index.html");
        if (System.IO.File.Exists(spa))
            return PhysicalFile(spa, "text/html; charset=utf-8");

        // Fallback legacy: build viejo en /dist/
        var spaDist = Path.Combine(_env.WebRootPath, "dist", "index.html");
        if (System.IO.File.Exists(spaDist))
            return Redirect("~/dist/");

        return View();
    }

    public IActionResult Buscar()
    {
        return Redirect("~/oficiantes");
    }

    public IActionResult Login()
    {
        return Redirect("~/login");
    }

    public IActionResult Registrar()
    {
        return Redirect("~/registrar");
    }

    public IActionResult VueApp()
    {
        return Redirect("~/");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
