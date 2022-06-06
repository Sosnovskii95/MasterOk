#nullable disable
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterOk.Controllers
{
    [Authorize(Roles = "admin")]
    public class DeliveryMethodsController : Controller
    {
        private readonly DataBaseContext _context;

        public DeliveryMethodsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: DeliveryMethods
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeliveryMethods.ToListAsync());
        }

        // GET: DeliveryMethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMethod = await _context.DeliveryMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }

            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeliveryMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleDeliveryMethod")] DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMethod = await _context.DeliveryMethods.FindAsync(id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }
            return View(deliveryMethod);
        }

        // POST: DeliveryMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleDeliveryMethod")] DeliveryMethod deliveryMethod)
        {
            if (id != deliveryMethod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryMethodExists(deliveryMethod.Id))
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
            return View(deliveryMethod);
        }

        // GET: DeliveryMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMethod = await _context.DeliveryMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }

            return View(deliveryMethod);
        }

        // POST: DeliveryMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryMethod = await _context.DeliveryMethods.FindAsync(id);
            _context.DeliveryMethods.Remove(deliveryMethod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryMethodExists(int id)
        {
            return _context.DeliveryMethods.Any(e => e.Id == id);
        }
    }
}
