using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var questoes = _context.Questao.Where(q => q.QuestionarioId == _idQuestionario).OrderBy(q => q.Id);
            return View(questoes);
        }

        public IActionResult BackToQuestionario()
        {
            return RedirectToAction("Index", "Questionarios");
        }

        public IActionResult ManagerAlternativa(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("IdQuestao", id.ToString());
            return RedirectToAction("Index", "Alternativas");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questao = await _context.Questao.FindAsync(id);
            _context.Questao.Remove(questao);
            _context.Alternativa.RemoveRange(_context.Alternativa.Where(a => a.QuestaoId == id));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestaoExists(int id)
        {
            return _context.Questao.Any(e => e.Id == id);
        }
    }
}
