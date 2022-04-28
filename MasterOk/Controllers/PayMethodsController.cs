#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;

namespace MasterOk.Controllers
{
    public class PayMethodsController : Controller
    {
        private readonly DataBaseContext _context;

        public PayMethodsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: PayMethods
        public async Task<IActionResult> Index()
        {
            return View(await _context.PayMethods.ToListAsync());
        }

        // GET: PayMethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payMethod = await _context.PayMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payMethod == null)
            {
                return NotFound();
            }

            return View(payMethod);
        }

        // GET: PayMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PayMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitlePayMethod")] PayMethod payMethod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(payMethod);
        }

        // GET: PayMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payMethod = await _context.PayMethods.FindAsync(id);
            if (payMethod == null)
            {
                return NotFound();
            }
            return View(payMethod);
        }

        // POST: PayMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitlePayMethod")] PayMethod payMethod)
        {
            if (id != payMethod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayMethodExists(payMethod.Id))
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
            return View(payMethod);
        }

        // GET: PayMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payMethod = await _context.PayMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payMethod == null)
            {
                return NotFound();
            }

            return View(payMethod);
        }

        // POST: PayMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payMethod = await _context.PayMethods.FindAsync(id);
            _context.PayMethods.Remove(payMethod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayMethodExists(int id)
        {
            return _context.PayMethods.Any(e => e.Id == id);
        }
    }
}
