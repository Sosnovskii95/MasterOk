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
using MasterOk.Models.FilesModify;
using MasterOk.Models.FilterSortViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    //[Authorize(Roles = "user")]
    public class ProductsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _webHost;

        public ProductsController(DataBaseContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: Products
        public async Task<IActionResult> Index(ESortModelProduct sort)
        {
            IQueryable<Product> dataBaseContext = _context.Products.Include(s => s.SubCategory);

            dataBaseContext = sort switch
            {
                ESortModelProduct.IdAsc => dataBaseContext.OrderBy(s => s.Id),
                ESortModelProduct.IdDesc => dataBaseContext.OrderByDescending(s => s.Id),
                ESortModelProduct.CountAsc => dataBaseContext.OrderBy(s => s.CountStoreProduct),
                ESortModelProduct.CountDesc => dataBaseContext.OrderByDescending(s => s.CountStoreProduct),
                ESortModelProduct.DescriptionAsc => dataBaseContext.OrderBy(s => s.DescriptionProduct),
                ESortModelProduct.DescriptionDesc => dataBaseContext.OrderByDescending(s => s.DescriptionProduct),
                ESortModelProduct.PriceAsc => dataBaseContext.OrderBy(s => s.Price),
                ESortModelProduct.PriceDesc => dataBaseContext.OrderByDescending(s => s.Price),
                ESortModelProduct.SubCategoryAsc => dataBaseContext.OrderBy(s => s.SubCategory.TitleSubCategory),
                ESortModelProduct.SubCategoryDesc => dataBaseContext.OrderByDescending(s => s.SubCategory.TitleSubCategory),
                ESortModelProduct.TitleAsc => dataBaseContext.OrderBy(s => s.TitleProduct),
                ESortModelProduct.TitleDesc => dataBaseContext.OrderByDescending(s => s.TitleProduct),
                ESortModelProduct.WarrantyAsc => dataBaseContext.OrderBy(s => s.Warranty),
                ESortModelProduct.WarrantyDesc => dataBaseContext.OrderByDescending(s => s.Warranty),
                _ => dataBaseContext
            };

            return View(new SortViewModelProduct
            {
                Products = await dataBaseContext.ToListAsync(),
                SortModelProduct = new SortModelProduct(sort)
            });
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            var nameImage = await _context.PathImages.Where(i => i.Product == product).Where(p => p.PathNameImage != "imagenot.jpg").ToListAsync();
            product.NameImages = nameImage;
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "TitleSubCategory");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleProduct,DescriptionProduct,Warranty,Price,SubCategoryId, CountStoreProduct")] Product product, IFormFileCollection nameImages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();

                List<PathImage> pathImage = new List<PathImage>();

                if (nameImages.Count > 0)
                {
                    Dictionary<string, string> listNameFiles = await ChangeFiles.SaveCreateUploadFiles(product.Id, _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(product), nameImages);

                    foreach (var file in listNameFiles)
                    {
                        pathImage.Add(new PathImage
                        {
                            Product = product,
                            PathNameImage = file.Key,
                            TypeImage = file.Value
                        });
                    }
                }
                else
                {
                    pathImage.Add(PathImageExtensions.GetDefaultPathNameFile(product));
                }

                _context.AddRange(pathImage);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "TitleSubCategory", product.SubCategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(n => n.NameImages).FirstOrDefaultAsync(i => i.Id == id);

            foreach (var item in product.NameImages)
            {
                if (item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                {
                    product.NameImages.Remove(item);
                }
            }

            if (product == null)
            {
                return NotFound();
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "TitleSubCategory", product.SubCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleProduct,DescriptionProduct,Warranty,Price,SubCategoryId, CountStoreProduct")] Product product, IFormFileCollection nameImages)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);

                    if (nameImages.Count > 0)
                    {
                        var pathNameImages = await _context.PathImages.Where(i => i.ProductId == product.Id).ToListAsync();

                        foreach (var item in pathNameImages)
                        {
                            if (item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                            {
                                _context.Remove(item);
                            }
                        }

                        Dictionary<string, string> listNameFiles = await ChangeFiles.SaveCreateUploadFiles(product.Id,
                            _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(product), nameImages);

                        foreach (var nameFiles in listNameFiles)
                        {
                            _context.Add(new PathImage
                            {
                                Product = product,
                                PathNameImage = nameFiles.Key,
                                TypeImage = nameFiles.Value
                            });
                        }
                    }
                    else
                    {
                        var pathImagesDb = await _context.PathImages.Where(p => p.ProductId == id).ToListAsync();

                        if (pathImagesDb.Count == 0)
                        {
                            _context.Add(PathImageExtensions.GetDefaultPathNameFile(product));
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "TitleSubCategory", product.SubCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            var nameImage = await _context.PathImages.Where(p => p.Product == product).Where(o => o.PathNameImage != "imagenot.jpg").ToListAsync();
            product.NameImages = nameImage;
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.Include(p => p.NameImages).FirstOrDefaultAsync(i => i.Id == id);

            foreach (var item in product.NameImages)
            {
                if (!item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                {
                    ChangeFiles.DeleteFiles(product.Id, _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(product), item.PathNameImage);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<VirtualFileResult> GetImage(int id)
        {
            if (id != null)
            {
                PathImage image = await _context.PathImages.FindAsync(id);
                if (image != null)
                {
                    string current = PathImageExtensions.GetDirectorySaveFile(image.Product) + image.ProductId;
                    return File(Path.Combine("~" + current, image.PathNameImage), image.TypeImage, image.PathNameImage);
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public async Task<IActionResult> DeleteImage(int id)
        {
            PathImage image = new PathImage();
            if (id != null)
            {
                image = await _context.PathImages.FindAsync(id);
                if (image != null)
                {
                    ChangeFiles.DeleteFiles(image.ProductId.Value, _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(image.Product), image.PathNameImage);

                    _context.PathImages.Remove(image);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Edit), new { id = image.ProductId.Value });
        }
    }
}
