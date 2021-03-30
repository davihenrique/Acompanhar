using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acompanhar.Data;
using Acompanhar.Models;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Questionario.ToListAsync());
        }

        // GET: Questionarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionario = await _context.Questionario
                .FirstOrDefaultAsync(m => m.QuestionarioId == id);
            if (questionario == null)
            {
                return NotFound();
            }

            return View(questionario);
        }

        // GET: Questionarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Questionarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionarioId,IdProfessor,Tema")] Questionario questionario)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> Edit(int id, [Bind("QuestionarioId,IdProfessor,Tema")] Questionario questionario)
        {
            if (id != questionario.QuestionarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionarioExists(questionario.QuestionarioId))
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
                .FirstOrDefaultAsync(m => m.QuestionarioId == id);
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
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionarioExists(int id)
        {
            return _context.Questionario.Any(e => e.QuestionarioId == id);
        }
    }
}
