using Acompanhar.Models;
using Acompanhar.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Acompanhar.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Teacher() => View();

        public IActionResult Administrador() => View();

        public IActionResult About() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FazerQuestioanrio(RealizaQuestionario realizaQuestionario)
        {
            HttpContext.Session.SetString("Code", (realizaQuestionario.Code).ToString());
            return RedirectToAction("Index", "RealizaQuestionario");
        }

        [ResponseCache(Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
