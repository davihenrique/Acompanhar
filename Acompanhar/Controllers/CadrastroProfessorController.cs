using Acompanhar.Data;
using Acompanhar.Models;
using Acompanhar.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acompanhar.Controllers
{
    public class CadrastroProfessorController : Controller
    {
        private readonly AcompanharContext _context;

        public CadrastroProfessorController(AcompanharContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login.Equals("yes"))
            {
                return View(await _context.Professor.ToListAsync());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Email, Password")] GenericLoginViewModel genericLogin)
        {
            if (genericLogin.Email.Equals("123") && genericLogin.Password.Equals("123"))
            {
                HttpContext.Session.SetString("Login", "yes");
            }
            else
            {
                HttpContext.Session.SetString("Login", "no");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }
            return View(professor);
        }

        public IActionResult Create()
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha")] Professor professor)
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Add(professor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(professor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Senha")] Professor professor)
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != professor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorExists(professor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(professor);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
            {
                return RedirectToAction("Index", "Home");
            }

            var professor = await _context.Professor.FindAsync(id);

            List<Questionario> questionarios = (List<Questionario>)_context.Questionario.Where(q => q.ProfessorId == id).ToList();
            List<Questao> questoes;

            foreach (Questionario q in questionarios)
            {
                questoes = (List<Questao>)_context.Questao.Where(qe => qe.QuestionarioId == q.Id).ToList();
                foreach (Questao qe in questoes)
                {
                    _context.Alternativa.RemoveRange(_context.Alternativa.Where(a => a.QuestaoId == qe.Id));
                }
                _context.Questao.RemoveRange(questoes);
            }
            _context.Questionario.RemoveRange(questionarios);

            _context.Professor.Remove(professor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProfessorExists(int id)
        {
            return _context.Professor.Any(e => e.Id == id);
        }
    }
}
