using Acompanhar.Data;
using Acompanhar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace Acompanhar.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly AcompanharContext _context;

        int IdProfessor;
        public ProfessorController(AcompanharContext context)
        {
            _context = context;
        }

        private async Task<Professor> AutentificarAsync()
        {
            string email = HttpContext.Session.GetString("Email");
            string senha = HttpContext.Session.GetString("Senha");

           List<Professor> professor =  await _context.Professor.ToListAsync();


            foreach(Professor p in professor)
            {
                if(p.Email.Equals(email))
                {
                    return p;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.Email = "null";
            string email = HttpContext.Session.GetString("Email");
            string senha = HttpContext.Session.GetString("Senha");
            bool emailValidado = false;
            List<Professor> professor = await _context.Professor.ToListAsync();
          
            foreach (Professor p in professor)
            {
                if (p.Email.Equals(email) && p.Senha.Equals(senha))
                {
                    emailValidado = true;
                    ViewBag.Id = p.Id;
                    ViewBag.Nome = p.Nome;
                    IdProfessor = p.Id;
                }
                
            }
            if (!emailValidado)
            {
                return RedirectToAction("Professor", "Home");
            }
            else
            {
                HttpContext.Session.SetString("IdProfessor",IdProfessor.ToString());
                return RedirectToAction("Index", "Questionarios");

            }            
        }
    }
}
