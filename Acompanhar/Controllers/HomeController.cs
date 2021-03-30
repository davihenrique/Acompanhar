using Acompanhar.Data;
using Acompanhar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(GenericLogin genericLogin)
        {

            HttpContext.Session.SetString("Email", genericLogin.Email);
            HttpContext.Session.SetString("Senha", genericLogin.Senha);


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
