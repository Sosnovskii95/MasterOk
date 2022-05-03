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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    public class ProductChecksController : Controller
    {
        private readonly DataBaseContext _context;

        public ProductChecksController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: ProductChecks
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.ProductChecks.Include(p => p.Client).Include(p => p.DeliveryMethod).Include(p => p.PayMethod).Include(p => p.StateOrder).Include(p => p.User).ThenInclude(s => s.Staff);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: ProductChecks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCheck = await _context.ProductChecks
                .Include(p => p.Client)
                .Include(p => p.DeliveryMethod)
                .Include(p => p.PayMethod)
                .Include(p => p.StateOrder)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCheck == null)
            {
                return NotFound();
            }

            return View(productCheck);
        }

        // GET: ProductChecks/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "TitleDeliveryMethod");
            ViewData["PayMethodId"] = new SelectList(_context.PayMethods, "Id", "TitlePayMethod");
            ViewData["StateOrderId"] = new SelectList(_context.StateOrders, "Id", "TitleState");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ProductChecks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateTimeSale,StateOrderId,ClientId,UserId,PayMethodId,DeliveryMethodId")] ProductCheck productCheck)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCheck);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", productCheck.ClientId);
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Id", productCheck.DeliveryMethodId);
            ViewData["PayMethodId"] = new SelectList(_context.PayMethods, "Id", "Id", productCheck.PayMethodId);
            ViewData["StateOrderId"] = new SelectList(_context.StateOrders, "Id", "Id", productCheck.StateOrderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", productCheck.UserId);
            return View(productCheck);
        }

        // GET: ProductChecks/Edit/5
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCheck = await _context.ProductChecks.Include(p => p.ProductSolds).ThenInclude(p => p.Product).Include(c => c.Client).Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
            if (productCheck == null)
            {
                return NotFound();
            }


            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "TitleDeliveryMethod", productCheck.DeliveryMethodId);
            ViewData["PayMethodId"] = new SelectList(_context.PayMethods, "Id", "TitlePayMethod", productCheck.PayMethodId);
            ViewData["StateOrderId"] = new SelectList(_context.StateOrders, "Id", "TitleState", productCheck.StateOrderId);
            return View(productCheck);
        }

        // POST: ProductChecks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Edit(int id, ProductCheck productCheck)
        {
            if (id != productCheck.Id)
            {
                return NotFound();
            }

            var userId = Convert.ToInt32(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            ProductCheck productCheckUpdate = await _context.ProductChecks.FindAsync(id);

            if (productCheckUpdate != null)
            {
                productCheckUpdate.StateOrderId = productCheck.StateOrderId;
                productCheckUpdate.PayMethodId = productCheck.PayMethodId;
                productCheckUpdate.DeliveryMethodId = productCheck.DeliveryMethodId;
                productCheckUpdate.UserId = userId;

                _context.Update(productCheckUpdate);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            productCheck = await _context.ProductChecks.Include(p => p.ProductSolds).ThenInclude(p => p.Product).Include(c => c.Client).Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);

            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "Id", productCheck.DeliveryMethodId);
            ViewData["PayMethodId"] = new SelectList(_context.PayMethods, "Id", "Id", productCheck.PayMethodId);
            ViewData["StateOrderId"] = new SelectList(_context.StateOrders, "Id", "Id", productCheck.StateOrderId);
            return View(productCheck);
        }

        // GET: ProductChecks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCheck = await _context.ProductChecks
                .Include(p => p.Client)
                .Include(p => p.DeliveryMethod)
                .Include(p => p.PayMethod)
                .Include(p => p.StateOrder)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCheck == null)
            {
                return NotFound();
            }

            return View(productCheck);
        }

        // POST: ProductChecks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCheck = await _context.ProductChecks.FindAsync(id);
            _context.ProductChecks.Remove(productCheck);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCheckExists(int id)
        {
            return _context.ProductChecks.Any(e => e.Id == id);
        }
    }
}
