using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Acompanhar.Data;
using Acompanhar.Models;
using Microsoft.AspNetCore.Http;
using Acompanhar.ViewModels;
using System.Collections.ObjectModel;

namespace Acompanhar.Controllers
{
    public class QuestionariosController : Controller
    {
        private readonly AcompanharContext _context;
        public QuestionariosController(AcompanharContext context)
        {
            _context = context;
        }

        public IActionResult Results(int? id)
        {

            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            {
                return NotFound();
            }

            List<Tarefa> tarefas = _context.Tarefa.Where(t => t.QuestionarioId==id).ToList();

            int count = _context.Tarefa.Count();

            double sum=0;

            foreach(Tarefa t in tarefas)
            {
                sum += t.Nota;
            }

            ViewData["Media"] = sum / count;

            return View(tarefas);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexAsync([Bind("Email, Password")] GenericLoginViewModel genericLogin)
        {
            int ProfessorId = 0;
            bool Validlogin = false;

            List<Professor> professor = await _context.Professor.ToListAsync();

            foreach (Professor p in professor)
            {
                if (p.Email.Equals(genericLogin.Email) && p.Senha.Equals(genericLogin.Password))
                {
                    Validlogin = true;
                    ViewBag.Id = p.Id;
                    ViewBag.Nome = p.Nome;
                    ProfessorId = p.Id;
                }

            }
            if (!Validlogin)
            {
                return RedirectToAction("Teacher", "Home");
            }
            else
            {
                HttpContext.Session.SetString("IdProfessor", ProfessorId.ToString());
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Index()
        {
            try
            {
                var _questionarios = _context.Questionario.Where(q => q.ProfessorId == int.Parse(HttpContext.Session.GetString("IdProfessor")));
                return View(_questionarios);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public IActionResult Exit()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                if (cookie == ".AspNetCore.Session")
                    Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Teacher", "Home");
        }

        public IActionResult Questao(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("IdQuestionario", id.ToString());
            return RedirectToAction("Index", "Questoes");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            {
                return NotFound();
            }
            var questionario = await _context.Questionario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionario == null)
            {
                return NotFound();
            }
            return View(questionario);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,ProfessorId,Tema")] Questionario questionario)
        {
            int _IdProfessor;
            try
            {
                _IdProfessor = int.Parse(HttpContext.Session.GetString("IdProfessor"));
            }
            catch (Exception)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                questionario.ProfessorId = _IdProfessor;
                _context.Add(questionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            {
                return NotFound();
            }
            var questionario = await _context.Questionario.FindAsync(id);
            if (questionario == null)
            {
                return NotFound();
            }
            return View(questionario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdProfessor,Tema")] Questionario questionario)
        {
            int _IdProfessor;
            try
            {
                _IdProfessor = int.Parse(HttpContext.Session.GetString("IdProfessor"));
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (id != questionario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                questionario.ProfessorId = _IdProfessor;
                try
                {
                    _context.Update(questionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionarioExists(questionario.Id))
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
            return View(questionario);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            {
                return NotFound();
            }

            var questionario = await _context.Questionario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionario == null)
            {
                return NotFound();
            }

            return View(questionario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionario = await _context.Questionario.FindAsync(id);
            _context.Questionario.Remove(questionario);

            List<Questao> questoes = (List<Questao>)_context.Questao.Where(q => q.QuestionarioId == id).ToList();

            foreach (Questao q in questoes)
            {
                _context.Alternativa.RemoveRange(_context.Alternativa.Where(a => a.QuestaoId == q.Id));
            }

            _context.Questao.RemoveRange(_context.Questao.Where((System.Linq.Expressions.Expression<Func<Questao, bool>>)(q => q.QuestionarioId == id)));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool QuestionarioExists(int id)
        {
            return _context.Questionario.Any(e => e.Id == id);
        }
    }
}
