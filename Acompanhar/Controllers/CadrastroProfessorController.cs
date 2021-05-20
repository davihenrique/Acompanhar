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

        public IActionResult Exit()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                if (cookie == ".AspNetCore.Session")
                    Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Administrador", "Home");
        }

        public IActionResult EditAdministrator()
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

            Administrador admin = _context.Administrador.Find(1);


            if (admin == null)
            {
                return NotFound();
            }


            return View(admin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdministratorAsync([Bind("Id,Email,Senha")] Administrador admin)
        {
            admin.Id = 1;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
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

            List<Administrador> admins = _context.Administrador.Where(a => a.Id == 1).ToList();
            bool login = false;

            foreach (Administrador a in admins)
            {
                if (genericLogin.Email.Equals(a.Email) && genericLogin.Password.Equals(a.Senha))
                {
                    login = true;
                }
            }

            if (login)
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

        public async Task<IActionResult> EditPasswordAsync(int? id)
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
        public async Task<IActionResult> EditPassword(int id,[Bind("Id,Nome,Email,Senha")] Professor professor)
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

                _context.Tarefa.RemoveRange(_context.Tarefa.Where(t => t.QuestionarioId == q.Id));
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
