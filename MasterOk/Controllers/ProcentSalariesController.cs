using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    [Authorize(Roles = "admin, clientmanager")]
    public class ProcentSalariesController : Controller
    {
        private readonly DataBaseContext _context;

        public ProcentSalariesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: ProcentSalaries
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcentSalaries.ToListAsync());
        }

        // GET: ProcentSalaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProcentSalaries == null)
            {
                return NotFound();
            }

            var procentSalary = await _context.ProcentSalaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procentSalary == null)
            {
                return NotFound();
            }

            return View(procentSalary);
        }

        // GET: ProcentSalaries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcentSalaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleProcentSalary,ValueProcentSalary")] ProcentSalary procentSalary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(procentSalary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(procentSalary);
        }

        // GET: ProcentSalaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProcentSalaries == null)
            {
                return NotFound();
            }

            var procentSalary = await _context.ProcentSalaries.FindAsync(id);
            if (procentSalary == null)
            {
                return NotFound();
            }
            return View(procentSalary);
        }

        // POST: ProcentSalaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleProcentSalary,ValueProcentSalary")] ProcentSalary procentSalary)
        {
            if (id != procentSalary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(procentSalary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcentSalaryExists(procentSalary.Id))
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
            return View(procentSalary);
        }

        // GET: ProcentSalaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProcentSalaries == null)
            {
                return NotFound();
            }

            var procentSalary = await _context.ProcentSalaries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procentSalary == null)
            {
                return NotFound();
            }

            return View(procentSalary);
        }

        // POST: ProcentSalaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProcentSalaries == null)
            {
                return Problem("Entity set 'DataBaseContext.ProcentSalaries'  is null.");
            }
            var procentSalary = await _context.ProcentSalaries.FindAsync(id);
            if (procentSalary != null)
            {
                _context.ProcentSalaries.Remove(procentSalary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcentSalaryExists(int id)
        {
            return (_context.ProcentSalaries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
