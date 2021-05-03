using Acompanhar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Acompanhar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Professor()
        {
            return View();
        }

        public IActionResult Administrador()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(GenericLogin genericLogin)
        {

            HttpContext.Session.SetString("Email", genericLogin.Email);
            HttpContext.Session.SetString("Senha", genericLogin.Senha);

            return RedirectToAction("Index", "Professor");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginAdminstrador(GenericLoginAdministrator genericLoginAdministrator)
        {
            HttpContext.Session.SetString("Name", genericLoginAdministrator.Name);
            HttpContext.Session.SetString("Senha", genericLoginAdministrator.Senha);

            string name = HttpContext.Session.GetString("Name");
            string pass = HttpContext.Session.GetString("Senha");

            return RedirectToAction("Index", "CadrastroProfessor");
        }




        public IActionResult Sobre()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
