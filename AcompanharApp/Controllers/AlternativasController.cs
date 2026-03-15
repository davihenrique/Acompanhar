using AcompanharApp.Data;
using AcompanharApp.Enums;
using AcompanharApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcompanharApp.Controllers
{
    public class AlternativasController : Controller
    {
        private readonly AcompanharContext _context;
        public AlternativasController(AcompanharContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int _QuestaoId;
            try
            {
                _QuestaoId = int.Parse(HttpContext.Session.GetString("IdQuestao"));

            }
            catch (Exception)
            {
                return NotFound();
            }

            var questao = _context.Questao.Include(q => q.Alternativas).Where(q => q.Id == _QuestaoId);

            var alternativas = questao.FirstOrDefault().Alternativas;

            return View(alternativas);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            {
                return NotFound();
            }

            var alternativa = await _context.Alternativa
                .FirstOrDefaultAsync(m => m.Id == id);

            if (alternativa == null)
            {
                return NotFound();
            }

            return View(alternativa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestaoId,Rotulo,Afirmacao,Veracidade")] Alternativa alternativa)
        {
            int _QuestaoId;
            int _index;
            try
            {
                _QuestaoId = int.Parse(HttpContext.Session.GetString("IdQuestao"));
                _index = _context.Alternativa.Count(a => a.QuestaoId == _QuestaoId);

            }
            catch (Exception)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (_index < Enum.GetNames(typeof(Label)).Length)
                {
                    Label Label = (Label)_index;
                    alternativa.Rotulo = Label.ToString();

                    alternativa.QuestaoId = _QuestaoId;
                    _context.Add(alternativa);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alternativa);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            {
                return NotFound();
            }
            var alternativa = await _context.Alternativa.FindAsync(id);
            if (alternativa == null)
            {
                return NotFound();
            }
            return View(alternativa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestaoId,Rotulo,Afirmacao,Veracidade")] Alternativa alternativa)
        {
            if (id != alternativa.Id)
            {
                return NotFound();
            }

            int _QuestaoId;
            try
            {
                _QuestaoId = int.Parse(HttpContext.Session.GetString("IdQuestao"));

            }
            catch (Exception)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    alternativa.QuestaoId = _QuestaoId;
                    _context.Update(alternativa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlternativaExists(alternativa.Id))
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
            return View(alternativa);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            {
                return NotFound();
            }

            var alternativa = await _context.Alternativa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alternativa == null)
            {
                return NotFound();
            }

            return View(alternativa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alternativa = await _context.Alternativa.FindAsync(id);
            _context.Alternativa.Remove(alternativa);
            await _context.SaveChangesAsync();

            int _QuestaoId;
            int _quant;
            try
            {
                _QuestaoId = int.Parse(HttpContext.Session.GetString("IdQuestao"));
                _quant = _context.Alternativa.Count(a => a.QuestaoId == _QuestaoId);
            }
            catch (Exception)
            {
                return NotFound();
            }

            var alternativasList = _context.Alternativa.Where(a => a.QuestaoId == _QuestaoId).OrderBy(a => a.Id).ToList();
            Label Label;

            for (int i = 1; i <= alternativasList.Count; i++)
            {
                Label = (Label)i;
                alternativasList[i].Rotulo = Label.ToString();

                _context.Update(alternativasList[i]);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Create() => View();

        public IActionResult BackToQuestao() => RedirectToAction("Index", "Questoes");

        private bool AlternativaExists(int id) => _context.Alternativa.Any(e => e.Id == id);
    }
}
