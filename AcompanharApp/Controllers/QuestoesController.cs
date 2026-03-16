using AcompanharApp.Data;
using AcompanharApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AcompanharApp.Controllers;

public class QuestoesController(AcompanharContext context) : Controller
{
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
        var questoes = context.Questao.Where(q => q.QuestionarioId == _idQuestionario).OrderBy(q => q.Id);
        return View(questoes);
    }

    public IActionResult BackToQuestionario() => RedirectToAction("Index", "Questionarios");

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
        var questao = await context.Questao
            .FirstOrDefaultAsync(m => m.Id == id);
        if (questao == null)
        {
            return NotFound();
        }
        return View(questao);
    }

    public IActionResult Create()
    {
        try
        {
            HttpContext.Session.GetString("IdQuestionario");
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
            context.Add(questao);
            await context.SaveChangesAsync();
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

        var questao = await context.Questao.FindAsync(id);
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
                context.Update(questao);
                await context.SaveChangesAsync();
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

        var questao = await context.Questao
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
        var questao = await context.Questao.FindAsync(id);
        context.Questao.Remove(questao);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool QuestaoExists(int id)
     => context.Questao.Any((System.Linq.Expressions.Expression<Func<Questao, bool>>)(e => e.Id == id));
}
