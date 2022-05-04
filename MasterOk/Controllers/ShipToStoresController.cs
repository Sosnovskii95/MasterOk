using Microsoft.AspNetCore.Mvc;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    [Authorize(Roles = "user" )]
    public class ShipToStoresController : Controller
    {
        private readonly DataBaseContext _context;

        public ShipToStoresController(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.DocShipToStores.Include(u => u.User).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "TitleProduct");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<int> productId, List<int> countShipProduct)
        {
            User user = await _context.Users.FindAsync(Convert.ToInt32(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType)));

            if (user == null)
            {
                return NotFound();
            }

            if (productId != null)
            {
                DocShipToStore docShipToStore = new DocShipToStore
                {
                    DateShip = DateTime.Now,
                    UserId = user.Id
                };
                _context.Add(docShipToStore);

                for (int i = 0; i < productId.Count; i++)
                {
                    Product product = await _context.Products.FindAsync(productId[i]);

                    if (product != null)
                    {
                        product.CountStoreProduct += countShipProduct[i];

                        _context.Add(new ShipToStore
                        {
                            CountShipProduct = countShipProduct[i],
                            ProductId = product.Id,
                            DocShipToStore = docShipToStore
                        });

                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                }

            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id != null)
            {
                return View(await _context.ShipToStores.Include(p => p.Product).Where(d => d.DocShipToStoreId == id).ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<int> id, List<int> countShip)
        {
            if (id.Count > 0)
            {
                for (int i = 0; i < id.Count; i++)
                {
                    ShipToStore shipToStore = await _context.ShipToStores.FindAsync(id[i]);

                    if (shipToStore != null)
                    {
                        Product product = await _context.Products.FindAsync(shipToStore.ProductId);

                        if (product != null)
                        {
                            product.CountStoreProduct -= shipToStore.CountShipProduct;
                            product.CountStoreProduct += countShip[i];

                            _context.Update(product);
                        }

                        shipToStore.CountShipProduct = countShip[i];
                        _context.Update(shipToStore);

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(int docShipToStoreId, int productId)
        {
            ShipToStore shipToStore = await _context.ShipToStores.Where(d => d.DocShipToStoreId == docShipToStoreId).Where(p => p.ProductId == productId).FirstOrDefaultAsync();

            if (shipToStore != null)
            {
                Product product = await _context.Products.FindAsync(shipToStore.ProductId);

                if (product != null)
                {
                    product.CountStoreProduct -= shipToStore.CountShipProduct;

                    _context.Update(product);
                }

                _context.Remove(shipToStore);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Edit), new { id = docShipToStoreId });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
