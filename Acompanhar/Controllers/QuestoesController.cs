using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acompanhar.Data;
using Microsoft.AspNetCore.Http;

namespace Acompanhar.Models
{
    public class QuestoesController : Controller
    {
        private readonly AcompanharContext _context;

        public QuestoesController(AcompanharContext context)
        {
            _context = context;
        }

        // GET: Questaos
        public IActionResult Index()
        {
            int _idQuestionario;
            try
            {
                _idQuestionario = int.Parse(HttpContext.Session.GetString("IdQuestionario"));

            }
            catch (Exception)
            {
                return NotFound();
            }
            var questoes = _context.Questao.Where(q => q.QuestionarioId == _idQuestionario);
            return View(questoes);
        }

        // GET: Questaos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var questao = await _context.Questao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questao == null)
            {
                return NotFound();
            }
            return View(questao);
        }

        // GET: Questaos/Create
        public IActionResult Create()
        {
            int _idQuestionario;
            try
            {
                _idQuestionario = int.Parse(HttpContext.Session.GetString("IdQuestionario"));
            }
            catch (Exception)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Questaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionarioId,Enunciado,Justificativa")] Questao questao)
        {
            int _idQuestionario;
            try
            {
                _idQuestionario = int.Parse(HttpContext.Session.GetString("IdQuestionario"));
            }
            catch (Exception)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                questao.QuestionarioId = _idQuestionario;
                _context.Add(questao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questao);
        }

        // GET: Questaos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questao = await _context.Questao.FindAsync(id);
            if (questao == null)
            {
                return NotFound();
            }
            return View(questao);
        }

        // POST: Questaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionarioId,Enunciado,Justificativa")] Questao questao)
        {
            int _idQuestionario;
            try
            {
                _idQuestionario = int.Parse(HttpContext.Session.GetString("IdQuestionario"));
            }
            catch (Exception)
            {
                return NotFound();
            }

            if (id != questao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    questao.QuestionarioId = _idQuestionario;
                    _context.Update(questao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestaoExists(questao.Id))
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
            return View(questao);
        }

        // GET: Questaos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questao = await _context.Questao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questao == null)
            {
                return NotFound();
            }

            return View(questao);
        }

        // POST: Questaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questao = await _context.Questao.FindAsync(id);
            _context.Questao.Remove(questao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestaoExists(int id)
        {
            return _context.Questao.Any(e => e.Id == id);
        }
    }
}
