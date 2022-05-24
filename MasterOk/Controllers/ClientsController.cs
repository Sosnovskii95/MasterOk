using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using MasterOk.Models.FilterSortViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    [Authorize(Roles = "admin, clientmanager")]
    public class ClientsController : Controller
    {
        private readonly DataBaseContext _context;

        public ClientsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index(ESortModelClient sort)
        {
            IQueryable<Client> dataBaseContext = _context.Clients.Include(c => c.ProcentSalary);

            dataBaseContext = sort switch
            {
                ESortModelClient.IdAsc => dataBaseContext.OrderBy(s => s.Id),
                ESortModelClient.IdDesc => dataBaseContext.OrderByDescending(s => s.Id),
                ESortModelClient.EmailAsc => dataBaseContext.OrderBy(s => s.EmailClient),
                ESortModelClient.EmailDesc => dataBaseContext.OrderByDescending(s => s.EmailClient),
                ESortModelClient.NameAsc => dataBaseContext.OrderBy(s => s.FirstLastNameClient),
                ESortModelClient.NameDesc => dataBaseContext.OrderByDescending(s => s.FirstLastNameClient),
                ESortModelClient.NumberPhoneAsc => dataBaseContext.OrderBy(s => s.NumberPhone),
                ESortModelClient.NumberPhoneDesc => dataBaseContext.OrderByDescending(s => s.NumberPhone),
                ESortModelClient.AddressAsc => dataBaseContext.OrderBy(s => s.Address),
                ESortModelClient.AddressDesc => dataBaseContext.OrderByDescending(s => s.Address),
                ESortModelClient.SalaryAsc => dataBaseContext.OrderBy(s => s.ProcentSalary.TitleProcentSalary),
                ESortModelClient.SalaryDesc => dataBaseContext.OrderByDescending(s => s.ProcentSalary.TitleProcentSalary),
                _ => dataBaseContext
            };

            return View(new SortViewModelClient
            {
                Clients = await dataBaseContext.ToListAsync(),
                SortModelClient = new SortModelClient(sort)
            });
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.ProcentSalary)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["ProcentSalaryId"] = new SelectList(_context.ProcentSalaries, "Id", "TitleProcentSalary");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmailClient,PasswordClient,FirstLastNameClient,NumberPhone,Address,ProcentSalaryId")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProcentSalaryId"] = new SelectList(_context.ProcentSalaries, "Id", "TitleProcentSalary", client.ProcentSalaryId);
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["ProcentSalaryId"] = new SelectList(_context.ProcentSalaries, "Id", "TitleProcentSalary", client.ProcentSalaryId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmailClient,PasswordClient,FirstLastNameClient,NumberPhone,Address,ProcentSalaryId")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            ViewData["ProcentSalaryId"] = new SelectList(_context.ProcentSalaries, "Id", "TitleProcentSalary", client.ProcentSalaryId);
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.ProcentSalary)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'DataBaseContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
