﻿#nullable disable
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
    //[Authorize(Roles = "user")]
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
            var dataBaseContext = _context.ProductChecks.Include(p => p.Client).Include(p => p.DeliveryMethod).Include(p => p.PayMethod).Include(p => p.StateOrder).Include(p => p.User);
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
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "TitleDeliveryMethod");
            ViewData["PayMethodId"] = new SelectList(_context.PayMethods, "Id", "TitlePayMethod");
            ViewData["StateOrderId"] = new SelectList(_context.StateOrders, "Id", "TitleState");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstLastNameStaff", userId);

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "TitleProduct");
            return View();
        }

        // POST: ProductChecks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCheck productCheck, List<int> productId, List<int> countSold)
        {
            if (productId.Count > 0)
            {
                List<ProductSold> productSold = new List<ProductSold>();

                for (int i = 0; i < productId.Count; i++)
                {
                    Product product = await _context.Products.FindAsync(productId[i]);

                    if (product != null)
                    {
                        var tempSold = countSold[i] <= product.CountStoreProduct ? countSold[i] : product.CountStoreProduct;

                        productSold.Add(new ProductSold
                        {
                            Product = product,
                            CountSold = tempSold,
                            PriceSold = product.Price,
                            TotalSold = tempSold * product.Price,
                            ProductCheck = productCheck
                        });

                        product.CountStoreProduct -= tempSold;
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                }
                productCheck.DateTimeSale = DateTime.Now;
                _context.Add(productCheck);

                _context.AddRange(productSold);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", productCheck.ClientId);
            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "TitleDeliveryMethod", productCheck.DeliveryMethodId);
            ViewData["PayMethodId"] = new SelectList(_context.PayMethods, "Id", "TitlePayMethod", productCheck.PayMethodId);
            ViewData["StateOrderId"] = new SelectList(_context.StateOrders, "Id", "TitleState", productCheck.StateOrderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstLastNameStaff", productCheck.UserId);

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "TitleProduct");
            return View(new ProductSold { ProductCheck = productCheck });
        }

        // GET: ProductChecks/Edit/5
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
        public async Task<IActionResult> Edit(int id, ProductCheck productCheck)
        {
            if (id != productCheck.Id)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(Convert.ToInt32(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType)));

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
                productCheckUpdate.UserId = user.Id;

                _context.Update(productCheckUpdate);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            productCheck = await _context.ProductChecks.Include(p => p.ProductSolds).ThenInclude(p => p.Product).Include(c => c.Client).Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);

            ViewData["DeliveryMethodId"] = new SelectList(_context.DeliveryMethods, "Id", "TitleDeliveryMethod", productCheck.DeliveryMethodId);
            ViewData["PayMethodId"] = new SelectList(_context.PayMethods, "Id", "TitlePayMethod", productCheck.PayMethodId);
            ViewData["StateOrderId"] = new SelectList(_context.StateOrders, "Id", "TitleState", productCheck.StateOrderId);
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

        [HttpPost]
        public async Task<IActionResult> GetClient(int idClient)
        {
            if (idClient != null)
            {
                Client client = await _context.Clients.FindAsync(idClient);

                if (idClient != null)
                {
                    return PartialView(client);
                }
            }
            return null;
        }
    }
}
