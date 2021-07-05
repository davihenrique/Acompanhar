using Acompanhar.Data;
using Acompanhar.Models;
using Acompanhar.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Acompanhar.Controllers
{
    public class CadrastroProfessorController : Controller
    {
        private readonly AcompanharContext _context;

        public CadrastroProfessorController(AcompanharContext context)
        {
            _context = context;
        }

        public IActionResult Exit()
        {
            var cookie = Request.Cookies.Keys.Where(c => c == ".AspNetCore.Session");
            if (cookie.Any())
            {
                Response.Cookies.Delete(cookie.FirstOrDefault());
            }

            return RedirectToAction("Administrador", "Home");
        }

        public IActionResult EditAdministrator()
        {
            try
            {
                if (IsLogin)
                {
                    var admin = _context.Administrador.Find(1);
                    return admin is null ? NotFound() : View(admin);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdministratorAsync([Bind("Id,Email,Senha")] Administrador admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (IsLogin)
                {
                    return View(await _context.Professor.ToListAsync());
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Email, Password")] GenericLoginViewModel genericLogin)
        {

            var admin = _context.Administrador.FirstOrDefault();

            if (genericLogin.Email.Equals(admin.Email) && genericLogin.Password.Equals(admin.Senha))
            {
                HttpContext.Session.SetString("Login", "yes");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                if (IsLogin)
                {
                    var professor = await _context.Professor.FirstOrDefaultAsync(m => m.Id == id);
                    if (professor == null)
                    {
                        return NotFound();
                    }
                    return View(professor);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
        {
            try
            {
                if (IsLogin)
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha")] Professor professor)
        {

            try
            {
                if (IsLogin)
                {

                    if (ModelState.IsValid)
                    {
                        _context.Add(professor);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(professor);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EditPasswordAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (IsLogin)
                {
                    var professor = await _context.Professor.FindAsync(id);

                    if (professor == null)
                    {
                        return NotFound();
                    }

                    return View(professor);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(int id, [Bind("Id,Nome,Email,Senha")] Professor professor)
        {

            if (id != professor.Id)
            {
                return NotFound();
            }

            try
            {

                if (HttpContext.Session.GetString("Login") != "yes")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
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

        public async Task<IActionResult> Edit(int? id)
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Senha")] Professor professor)
        {
            string login;
            try
            {
                login = HttpContext.Session.GetString("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            if (login != "yes")
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (IsLogin)
                {
                    var professor = await _context.Professor
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (professor == null)
                    {
                        return NotFound();
                    }

                    return View(professor);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {

                if (IsLogin)
                {
                    var professor = await _context.Professor.FindAsync(id);

                    _context.Professor.Remove(professor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        private bool IsLogin => HttpContext.Session.GetString("Login") == "yes";

        private bool ProfessorExists(int id)
        {
            return _context.Professor.Any(e => e.Id == id);
        }
    }
}
