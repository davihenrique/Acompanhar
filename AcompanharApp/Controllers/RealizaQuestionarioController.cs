using AcompanharApp.Data;
using AcompanharApp.Models;
using AcompanharApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcompanharApp.Controllers;
    public class RealizaQuestionarioController(AcompanharContext context) : Controller
    {
        public IActionResult Index()
        {
            QuestionarioCursorViewModel Cursor = new();
            try
            {
                Cursor.Code = int.Parse(HttpContext.Session.GetString("Code"));
                Cursor.Size = int.Parse(HttpContext.Session.GetString("Max"));
                Cursor.CurrentPoint = int.Parse(HttpContext.Session.GetString("Cursor"));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Questao> questoes = context.Questao.Where(q => q.QuestionarioId == Cursor.Code).Include(q => q.Alternativas).ToList();

            Questao qa;
            if (Cursor.CurrentPoint <= Cursor.Size)
            {
                qa = questoes[Cursor.CurrentPoint];
            }
            else
            {
                return RedirectToAction(nameof(Feedback));
            }

            HttpContext.Session.SetString("justification", qa.Justificativa);

            List<Alternativa> alternativasCorretas = qa.Alternativas.Where(a => a.Veracidade).ToList();

            string _correctAnswer = "";

            foreach (Alternativa a in alternativasCorretas)
            {
                _correctAnswer += a.Rotulo + " ";
            }

            HttpContext.Session.SetString("correctAnswer", _correctAnswer);

            QuestionarioViewModel q = new()
            {
                Question = qa.Enunciado,
                Alternatives = new List<AlternativeViewModel>()

            };

            foreach (var alternative in qa.Alternativas)
            {
                q.Alternatives.Add(new AlternativeViewModel
                {
                    Text = alternative.Afirmacao,
                    Label = alternative.Rotulo,
                    IsCorrect = alternative.Veracidade,
                    Selected = false
                });
            }



            HttpContext.Session.SetString("Cursor", (++Cursor.CurrentPoint).ToString());
            ViewData["QuestaoAtual"] = Cursor.CurrentPoint;
            ViewData["NumeroDeQuestao"] = Cursor.Size + 1;
            ViewData["Tema"] = HttpContext.Session.GetString("Tema");
            return View(q);
        }

        public IActionResult Feedback()
        {
            double res;

            try
            {
                res = (double)((100 * (HttpContext.Session.GetInt32("Pontos"))) / context.Questao.Count(q => q.QuestionarioId == int.Parse(HttpContext.Session.GetString("Code")))) / 10;
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["nota"] = @String.Format("{0:N1}", res).Replace(".", ",");
            HttpContext.Session.SetString("nota", res.ToString());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> FeedbackAsync([Bind("Message")] FeedbackViewModel feedbackViewModel)
        {
            Tarefa t = new();
            try
            {
                t.QuestionarioId = int.Parse(HttpContext.Session.GetString("Code"));
                t.Nota = double.Parse(HttpContext.Session.GetString("nota"));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            t.Messagem = feedbackViewModel.Message;

            context.Add(t);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Result(QuestionarioViewModel userResponse)
        {
            RespostaViewModel r = new()
            {
                Justification = HttpContext.Session.GetString("justification"),
                CorrectAnswer = HttpContext.Session.GetString("correctAnswer")
            };

            var selectedLabels = userResponse?.Alternatives?
                .Where(a => a.Selected)
                .Select(a => a.Label)
                .Where(label => !string.IsNullOrWhiteSpace(label))
                .ToList() ?? new List<string>();

            r.Checked = string.Join(" ", selectedLabels);

            string normalizedChecked = string.Join(" ", selectedLabels
                .Select(label => label.Trim())
                .OrderBy(label => label, StringComparer.OrdinalIgnoreCase));

            string normalizedCorrect = string.Join(" ", (r.CorrectAnswer ?? string.Empty)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(label => label.Trim())
                .OrderBy(label => label, StringComparer.OrdinalIgnoreCase));

            if (normalizedChecked.Equals(normalizedCorrect, StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Session.SetInt32("Pontos", (int)(HttpContext.Session.GetInt32("Pontos") + 1));
            }

            return View(r);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Code")] QuestionarioCursorViewModel cursor)
        {
            if (!context.Questionario.Any(q => q.Id == cursor.Code))
            {
                return RedirectToAction("Index", "Home");
            }
            Questionario q;
            try
            {
                cursor.Size = (context.Questao.Count(q => q.QuestionarioId == cursor.Code)) - 1;
                q = context.Questionario.Find(cursor.Code);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            cursor.CurrentPoint = 0;

            HttpContext.Session.SetString("Code", cursor.Code.ToString());
            HttpContext.Session.SetString("Max", (cursor.Size).ToString());
            HttpContext.Session.SetString("Cursor", cursor.CurrentPoint.ToString());

            HttpContext.Session.SetString("Tema", q.Tema);

            HttpContext.Session.SetInt32("Pontos", 0);

            return RedirectToAction(nameof(Index));
        }
    }
