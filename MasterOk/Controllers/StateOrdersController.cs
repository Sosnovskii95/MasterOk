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
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    //[Authorize(Roles = "user")]
    public class StateOrdersController : Controller
    {
        private readonly DataBaseContext _context;

        public StateOrdersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: StateOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.StateOrders.ToListAsync());
        }

        // GET: StateOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stateOrder = await _context.StateOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stateOrder == null)
            {
                return NotFound();
            }

            return View(stateOrder);
        }

        // GET: StateOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StateOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleState")] StateOrder stateOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stateOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stateOrder);
        }

        // GET: StateOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stateOrder = await _context.StateOrders.FindAsync(id);
            if (stateOrder == null)
            {
                return NotFound();
            }
            return View(stateOrder);
        }

        // POST: StateOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleState")] StateOrder stateOrder)
        {
            if (id != stateOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stateOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateOrderExists(stateOrder.Id))
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
            return View(stateOrder);
        }

        // GET: StateOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stateOrder = await _context.StateOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stateOrder == null)
            {
                return NotFound();
            }

            return View(stateOrder);
        }

        // POST: StateOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stateOrder = await _context.StateOrders.FindAsync(id);
            _context.StateOrders.Remove(stateOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StateOrderExists(int id)
        {
            return _context.StateOrders.Any(e => e.Id == id);
        }
    }
}
