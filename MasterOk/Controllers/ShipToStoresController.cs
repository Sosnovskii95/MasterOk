using Microsoft.AspNetCore.Mvc;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MasterOk.Models.FilterSortViewModels;

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

        public async Task<IActionResult> Index(ESortModelDocShip sort)
        {
            IQueryable<DocShipToStore> dataBaseContext = _context.DocShipToStores.Include(u => u.User);

            dataBaseContext = sort switch
            {
                ESortModelDocShip.IdAsc => dataBaseContext.OrderBy(s => s.Id),
                ESortModelDocShip.IdDesc => dataBaseContext.OrderByDescending(s => s.Id),
                ESortModelDocShip.DateAsc => dataBaseContext.OrderBy(s => s.DateShip),
                ESortModelDocShip.DateDesc => dataBaseContext.OrderByDescending(s => s.DateShip),
                ESortModelDocShip.UserAsc => dataBaseContext.OrderBy(s => s.User.FirstLastNameStaff),
                ESortModelDocShip.UserDesc => dataBaseContext.OrderByDescending(s => s.User.FirstLastNameStaff),
                _ => dataBaseContext
            };

            return View(new SortViewModelDocShip
            {
                DocShipToStores = await dataBaseContext.ToListAsync(),
                SortModelDocShip = new SortModelDocShip(sort)
            });
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
                ViewData["docShip"] = await _context.DocShipToStores.FindAsync(id);
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

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipToStores = await _context.ShipToStores.Include(p => p.Product)
                                                          .Where(d => d.DocShipToStoreId == id).ToListAsync();

            if (shipToStores == null)
            {
                return NotFound();
            }
            ViewData["docShip"] = await _context.DocShipToStores.FindAsync(id);

            return View(shipToStores);


            /*ShipToStore shipToStore = await _context.ShipToStores.Where(d => d.DocShipToStoreId == docShipToStoreId).Where(p => p.ProductId == productId).FirstOrDefaultAsync();

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
            }*/
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id != null)
            {
                var docShipToStores = await _context.DocShipToStores.Include(s => s.ShipToStores)
                                                                    .ThenInclude(p => p.Product)
                                                                    .FirstOrDefaultAsync(i => i.Id == id);

                if (docShipToStores != null)
                {
                    foreach (var item in docShipToStores.ShipToStores)
                    {
                        var product = await _context.Products.FindAsync(item.ProductId);

                        if (product != null)
                        {
                            product.CountStoreProduct -= item.CountShipProduct;
                            _context.Update(product);
                        }
                    }

                    _context.Remove(docShipToStores);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                return Redirect(HttpContext.Request.Headers.Referer);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            if (id != null)
            {
                var shipProduct = await _context.ShipToStores.Include(p => p.Product).FirstOrDefaultAsync(i => i.Id == id);
                if (shipProduct != null)
                {
                    var product = await _context.Products.FindAsync(shipProduct.ProductId);

                    if (product != null)
                    {
                        product.CountStoreProduct -= shipProduct.CountShipProduct;

                        _context.Update(product);
                    }

                    _context.Remove(shipProduct);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Edit), new { id = shipProduct.DocShipToStoreId });
                }
            }

            return NotFound();
        }
    }
}
