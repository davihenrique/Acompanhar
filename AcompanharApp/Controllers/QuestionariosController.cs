using AcompanharApp.Data;
using AcompanharApp.Models;
using AcompanharApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcompanharApp.Controllers;

public class QuestionariosController(AcompanharContext context) : Controller
{
    private readonly AcompanharContext context = context;

    public IActionResult Results(int? id)
    {

        if (id == null || HttpContext.Session.GetString("IdProfessor") == null)
        {
            return NotFound();
        }

        List<Tarefa> tarefas = context.Tarefa.Where(t => t.QuestionarioId == id).ToList();


        if (tarefas.Count < 1)
        {
            ViewData["Quantidade"] = 0;
            ViewData["Media"] = 0;
            ViewData["Max"] = 0;
            ViewData["Mediana"] = 0;

        }
        else
        {
            ViewData["Quantidade"] = tarefas.Count;
            ViewData["Media"] = @String.Format("{0:N1}", tarefas.Select(t => t.Nota).Average()).Replace(".", ",");
            ViewData["Max"] = @String.Format("{0:N1}", tarefas.Select(t => t.Nota).Max()).Replace(".", ",");
            if (tarefas.Count % 2 == 1)
            {
                ViewData["Mediana"] = @String.Format("{0:N1}", tarefas.OrderBy(t => t.Nota).Select(t => t.Nota).Skip(tarefas.Count() / 2).First()).Replace(".", ",");
            }
            else
            {
                ViewData["Mediana"] = @String.Format("{0:N1}", ((tarefas.OrderBy(t => t.Nota).Select(t => t.Nota).Skip(tarefas.Count() / 2).First()) + (tarefas.OrderBy(t => t.Nota).Select(t => t.Nota).Skip((tarefas.Count() - 1) / 2).First())) / 2).Replace(".", ",");
            }

        }

        return View(tarefas);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index([Bind("Email, Password")] GenericLoginViewModel genericLogin)
    {
        var professor = context.Professor.Where(p => p.Email.Equals(genericLogin.Email) && p.Senha.Equals(genericLogin.Password));

        if (professor.Any())
        {
            ViewBag.Id = professor.FirstOrDefault().Id;
            ViewBag.Nome = professor.FirstOrDefault().Nome;

            HttpContext.Session.SetString("IdProfessor", professor.FirstOrDefault().Id.ToString());
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction("Teacher", "Home");
    }
    public IActionResult Index()
    {
        try
        {
            var _questionarios = context.Questionario.Where(q => q.ProfessorId == int.Parse(HttpContext.Session.GetString("IdProfessor")));
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
        var questionario = await context.Questionario
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
            context.Add(questionario);
            await context.SaveChangesAsync();
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
        var questionario = await context.Questionario.FindAsync(id);
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
                context.Update(questionario);
                await context.SaveChangesAsync();
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

        var questionario = await context.Questionario
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
        var questionario = await context.Questionario.FindAsync(id);
        context.Questionario.Remove(questionario);

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private bool QuestionarioExists(int id) => context.Questionario.Any(e => e.Id == id);
}
