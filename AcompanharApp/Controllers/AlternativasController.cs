using AcompanharApp.Data;
using AcompanharApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcompanharApp.Controllers;

public class AlternativasController(AcompanharContext context) : Controller
{
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

        var questao = context.Questao.Include(q => q.Alternativas).Where(q => q.Id == _QuestaoId);

        var alternativas = questao.FirstOrDefault().Alternativas;

        return View(alternativas);
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            return NotFound();

        var alternativa = await context.Alternativa
            .FirstOrDefaultAsync(m => m.Id == id);

        if (alternativa == null)
            return NotFound();

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
            _index = context.Alternativa.Count(a => a.QuestaoId == _QuestaoId);

        }
        catch (Exception)
        {
            return NotFound();
        }

        char quintaLetra = GetLetter(5);

        if (ModelState.IsValid)
        {
            alternativa.Rotulo = GetLetter(_index).ToString();
            alternativa.QuestaoId = _QuestaoId;
            context.Add(alternativa);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(alternativa);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            return NotFound();

        var alternativa = await context.Alternativa.FindAsync(id);
        if (alternativa == null)
            return NotFound();

        return View(alternativa);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,QuestaoId,Rotulo,Afirmacao,Veracidade")] Alternativa alternativa)
    {
        if (id != alternativa.Id)
            return NotFound();

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
                context.Update(alternativa);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlternativaExists(alternativa.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(alternativa);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
            return NotFound();

        var alternativa = await context.Alternativa
            .FirstOrDefaultAsync(m => m.Id == id);
        if (alternativa == null)
            return NotFound();

        return View(alternativa);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var alternativa = await context.Alternativa.FindAsync(id);
        context.Alternativa.Remove(alternativa);
        await context.SaveChangesAsync();

        int _QuestaoId;
        int _quant;
        try
        {
            _QuestaoId = int.Parse(HttpContext.Session.GetString("IdQuestao"));
            _quant = context.Alternativa.Count(a => a.QuestaoId == _QuestaoId);
        }
        catch (Exception)
        {
            return NotFound();
        }

        var alternativasList = context.Alternativa.Where(a => a.QuestaoId == _QuestaoId).OrderBy(a => a.Id).ToList();

        foreach (var alt in alternativasList)
        {
            alt.Rotulo = GetLetter(alternativasList.IndexOf(alt)).ToString();
        }

        context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }


    public IActionResult Create() => View();

    public IActionResult BackToQuestao() => RedirectToAction("Index", "Questoes");

    private char GetLetter(int index) => (char)('A' + (index));

    private bool AlternativaExists(int id) => context.Alternativa.Any(e => e.Id == id);
}
