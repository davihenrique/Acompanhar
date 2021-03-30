using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Acompanhar.Data;
using Acompanhar.Models;
using Microsoft.AspNetCore.Http;

namespace Acompanhar.Controllers
{
    public class CadrastroProfessorController : Controller
    {
        private readonly AcompanharContext _context;

        public CadrastroProfessorController(AcompanharContext context)
        {
            _context = context;
        }

        private bool verificarLogin()
        {
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Senha = HttpContext.Session.GetString("Senha");


            if (ViewBag.Email != "admin" || ViewBag.Senha != "admin")
            {
                return false;
            }

            return true;
        }

        // GET: CadrastroProfessor
        public async Task<IActionResult> Index()
        {
            if (!verificarLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(await _context.Professor.ToListAsync());
        }

        // GET: CadrastroProfessor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!verificarLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }
            return View(professor);
        }

        // GET: Professors/Create
        public IActionResult Create()
        {

            if (!verificarLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Professors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha")] Professor professor)
        {
            if (!verificarLogin())
            {
                return RedirectToAction("Index", "Home");
            }


            if (ModelState.IsValid)
            {
                _context.Add(professor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(professor);
        }

        // GET: Professors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!verificarLogin())
            {
                return RedirectToAction("Index", "Home");
            }



            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            return View(professor);
        }

        // POST: Professors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Senha")] Professor professor)
        {

            if (!verificarLogin())
            {
                return RedirectToAction("Index", "Home");
            }



            if (id != professor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorExists(professor.Id))
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
            return View(professor);
        }

        // GET: Professors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!verificarLogin())
            {
                return RedirectToAction("Index", "Home");
            }


            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        // POST: Professors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (!verificarLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            var professor = await _context.Professor.FindAsync(id);
            _context.Professor.Remove(professor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessorExists(int id)
        {
            return _context.Professor.Any(e => e.Id == id);
        }
    }
}
