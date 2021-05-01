using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acompanhar.Data;
using Acompanhar.Models;
using Microsoft.AspNetCore.Http;


namespace Acompanhar.Controllers
{
    public class QuestionariosController : Controller
    {
        private readonly AcompanharContext _context;
        public QuestionariosController(AcompanharContext context)
        {
            _context = context;
        }

        // GET: Questionarios
        public IActionResult Index()
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

            var questionarios = _context.Questionario.Where(q => q.IdProfessor == _IdProfessor);
            return View(questionarios);
        }

        public IActionResult Exit()
        {
            foreach (var cookie in Request.Cookies.Keys)

            {
                if (cookie == ".AspNetCore.Session")
                    Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Professor", "Home");
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

        // GET: Questionarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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

        // GET: Questionarios/Create
        public IActionResult Create()
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

            return View();
        }

        // POST: Questionarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdProfessor,Tema")] Questionario questionario)
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
                questionario.IdProfessor = _IdProfessor;
                _context.Add(questionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questionario);
        }

        // GET: Questionarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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

        // POST: Questionarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                questionario.IdProfessor = _IdProfessor;
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

        // GET: Questionarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
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

        // POST: Questionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionario = await _context.Questionario.FindAsync(id);
            _context.Questionario.Remove(questionario);

            List<Questao> questoes = (List<Questao>)_context.Questao.Where(q => q.QuestionarioId == id).ToList();

            foreach(Questao q in questoes)
            {
                _context.Alternativa.RemoveRange(_context.Alternativa.Where(a => a.QuestaoId == q.Id));
            }

           _context.Questao.RemoveRange(_context.Questao.Where(q => q.QuestionarioId == id));

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionarioExists(int id)
        {
            return _context.Questionario.Any(e => e.Id == id);
        }
    }
}
