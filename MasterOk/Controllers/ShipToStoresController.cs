using Microsoft.AspNetCore.Mvc;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MasterOk.Controllers
{
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
            ViewBag.ProductId = new SelectList(await _context.Products.ToListAsync(), "Id", "TitleProduct");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<int> productId, List<int> countShipProduct)
        {
            if (productId != null)
            {
                DocShipToStore docShipToStore = new DocShipToStore
                {
                    DateShip = DateTime.Now
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

        [HttpPost]
        public async Task<IActionResult> Change(int id, int valueId, int docId)
        {
            if (id != null)
            {
                ShipToStore shipToStore = await _context.ShipToStores.FindAsync(id);

                if (shipToStore != null)
                {
                    Product product = await _context.Products.FindAsync(shipToStore.ProductId);

                    if (product != null)
                    {
                        product.CountStoreProduct -= shipToStore.CountShipProduct;
                        product.CountStoreProduct += valueId;

                        _context.Update(product);
                    }

                    shipToStore.CountShipProduct = valueId;
                    _context.Update(shipToStore);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Edit), new { id = docId });
                }
            }

            return null;
        }
    }
}
