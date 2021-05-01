using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Acompanhar.Data;
using Acompanhar.Models;
using Microsoft.AspNetCore.Http;
using Acompanhar.Repositories;
using Acompanhar.Enums;

namespace Acompanhar.Controllers
{
    public class AlternativasController : Controller
    {
        private readonly AcompanharContext _context;

        private readonly IAlternativaRepository _alternativaRepository;

        public AlternativasController(AcompanharContext context, IAlternativaRepository alternativaRepository)
        {
            _context = context;

            _alternativaRepository = alternativaRepository;
        }

        // GET: Alternativas
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
            var alternativas = _alternativaRepository.GetAlternativasOrder(_QuestaoId);
            return View(alternativas);
        }

        public IActionResult BackToQuestao()
        {
            return RedirectToAction("Index", "Questoes");
        }

        // GET: Alternativas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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

        // GET: Alternativas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alternativas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestaoId,Rotulo,Afirmacao,Verdadeira")] Alternativa alternativa)
        {
            int _QuestaoId;
            int _quant;
            try
            {
                _QuestaoId = int.Parse(HttpContext.Session.GetString("IdQuestao"));
                _quant = _alternativaRepository.GetAlternativaCont(_QuestaoId);

            }
            catch (Exception)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (_quant < Enum.GetNames(typeof(Rotulo)).Length)
                {
                    Rotulo rotulo = (Rotulo)_quant;
                    alternativa.Rotulo = rotulo.ToString();

                    alternativa.QuestaoId = _QuestaoId;
                    _context.Add(alternativa);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alternativa);
        }

        // GET: Alternativas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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

        // POST: Alternativas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestaoId,Rotulo,Afirmacao,Verdadeira")] Alternativa alternativa)
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

        // GET: Alternativas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
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

        // POST: Alternativas/Delete/5
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
                _quant = _alternativaRepository.GetAlternativaCont(_QuestaoId);
            }
            catch (Exception)
            {
                return NotFound();
            }

            List<Alternativa> alternativasList = (List<Alternativa>)_alternativaRepository.GetAlternativasOrder(_QuestaoId);
            Rotulo rotulo;

            for (int i = 0; i < alternativasList.Count; i++)
            {
                rotulo = (Rotulo)i;
                alternativasList[i].Rotulo = rotulo.ToString();

                _context.Update(alternativasList[i]);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AlternativaExists(int id)
        {
            return _context.Alternativa.Any(e => e.Id == id);
        }
    }
}
