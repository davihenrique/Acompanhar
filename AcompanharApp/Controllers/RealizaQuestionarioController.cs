using AcompanharApp.Data;
using AcompanharApp.Enums;
using AcompanharApp.Models;
using AcompanharApp.Repositories;
using AcompanharApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcompanharApp.Controllers
{
    public class RealizaQuestionarioController : Controller
    {
        private readonly AcompanharContext _context;
        private readonly IRealizaQuestionarioRepository _realizaQuestionarioRepository;
        public RealizaQuestionarioController(AcompanharContext context, IRealizaQuestionarioRepository realizaQuestionarioRepository)
        {
            _context = context;
            _realizaQuestionarioRepository = realizaQuestionarioRepository;
        }

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

            List<Questao> questoes = _context.Questao.Where(q => q.QuestionarioId == Cursor.Code).ToList();

            Questao qa;
            if (Cursor.CurrentPoint <= Cursor.Size)
            {
                qa = questoes[Cursor.CurrentPoint];
            }
            else
            {
                return RedirectToAction(nameof(Feedback));
            }

            List<Alternativa> alternativas = _context.Alternativa.Where(a => a.QuestaoId == qa.Id).ToList();

            HttpContext.Session.SetString("justification", qa.Justificativa);

            List<Alternativa> alternativasCorretas = _context.Alternativa.Where(a => a.QuestaoId == qa.Id).Where(a => a.Veracidade == true).ToList();
            string _correctAnswer = "";

            foreach (Alternativa a in alternativasCorretas)
            {
                _correctAnswer += a.Rotulo + " ";
            }

            HttpContext.Session.SetString("correctAnswer", _correctAnswer);

            QuestionarioViewModel q = new()
            {
                Question = qa.Enunciado

            };
            try
            {
                List<Alternativa> questaA = alternativas.Where(a => a.Rotulo == ((Label)0).ToString()).ToList();
                q.TextOption1St = questaA[0].Afirmacao;
                q.Label1St = ((Label)0).ToString();
            }
            catch (Exception)
            {
            }
            try
            {
                List<Alternativa> questaB = alternativas.Where(a => a.Rotulo == ((Label)1).ToString()).ToList();
                q.TextOption2Nd = questaB[0].Afirmacao;
                q.Label2Nd = ((Label)1).ToString();
            }
            catch (Exception)
            {
            }
            try
            {
                List<Alternativa> questaC = alternativas.Where(a => a.Rotulo == ((Label)2).ToString()).ToList();
                q.TextOption3Rd = questaC[0].Afirmacao;
                q.Label3Rd = ((Label)2).ToString();
            }
            catch (Exception)
            {
            }
            try
            {
                List<Alternativa> questaD = alternativas.Where(a => a.Rotulo == ((Label)3).ToString()).ToList();
                q.TextOption4Th = questaD[0].Afirmacao;
                q.Label4Th = ((Label)3).ToString();
            }
            catch (Exception)
            {
            }
            try
            {
                List<Alternativa> questaE = alternativas.Where(a => a.Rotulo == ((Label)4).ToString()).ToList();
                q.TextOption5Th = questaE[0].Afirmacao;
                q.Label5Th = ((Label)4).ToString();
            }
            catch (Exception)
            {
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
                res = (double)((100 * (HttpContext.Session.GetInt32("Pontos"))) / _context.Questao.Count(q => q.QuestionarioId == int.Parse(HttpContext.Session.GetString("Code")))) / 10;
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

            _context.Add(t);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Result([Bind("Option1St,Option2Nd,Option3Rd,Option4Th,Option5Th")] UserResponse userResponse)
        {
            Resposta r = new();
            r.Justification = HttpContext.Session.GetString("justification");
            r.CorrectAnswer = HttpContext.Session.GetString("correctAnswer");

            string Checked = "";

            if (userResponse.Option1St) { Checked += ((Label)0) + " "; }
            if (userResponse.Option2Nd) { Checked += ((Label)1) + " "; }
            if (userResponse.Option3Rd) { Checked += ((Label)2) + " "; }
            if (userResponse.Option4Th) { Checked += ((Label)3) + " "; }
            if (userResponse.Option5Th) { Checked += ((Label)4) + " "; }

            r.Checked = Checked;

            if (r.Checked.Equals(r.CorrectAnswer))
            {
                HttpContext.Session.SetInt32("Pontos", (int)(HttpContext.Session.GetInt32("Pontos") + 1));
            }

            return View(r);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Code")] QuestionarioCursorViewModel cursor)
        {
            if (!_realizaQuestionarioRepository.IsQuestionario(cursor.Code))
            {
                return RedirectToAction("Index", "Home");
            }
            Questionario q;
            try
            {
                cursor.Size = (_context.Questao.Count(q => q.QuestionarioId == cursor.Code)) - 1;
                q = _context.Questionario.Find(cursor.Code);

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
}
