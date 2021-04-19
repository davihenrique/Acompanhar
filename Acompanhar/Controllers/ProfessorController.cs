using Acompanhar.Data;
using Acompanhar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acompanhar.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly AcompanharContext _context;
        public ProfessorController(AcompanharContext context)
        {
            _context = context;
        }

        private async Task<Professor> AutentificarAsync()
        {
            // pegar email e senha
            string email = HttpContext.Session.GetString("Email");
            string senha = HttpContext.Session.GetString("Senha");

            // pesquisar no banco
           List<Professor> professor =  await _context.Professor.ToListAsync();


            // conferir senha
            foreach(Professor p in professor)
            {
                if(p.Email.Equals(email))
                {
                    return p;
                    //verificar senha
                }
                else
                {
                    return null;
                }
            }


            // retornar o professor
            //TESTE return new Professor(10,"Davi","d@gmail.com","123");
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
                }
                
            }
            if (!emailValidado)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }            
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
