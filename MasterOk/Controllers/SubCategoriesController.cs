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

namespace MasterOk.Controllers
{
    public class SubCategoriesController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _webHost;

        public SubCategoriesController(DataBaseContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: SubCategories
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.SubCategories.Include(s => s.Category);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: SubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            var nameImages = await _context.PathImages.Where(i => i.SubCategory == subCategory).Where(p => p.PathNameImage != "imagenot.jpg").ToListAsync();
            subCategory.NameImages = nameImages;
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // GET: SubCategories/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "TitleCategory");
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleSubCategory,CategoryId")] SubCategory subCategory, IFormFileCollection nameImages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subCategory);
                await _context.SaveChangesAsync();

                List<PathImage> pathImage = new List<PathImage>();

                if (nameImages.Count > 0)
                {
                    Dictionary<string, string> listNameFiles = await ChangeFiles.SaveCreateUploadFiles(subCategory.Id,
                        _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(subCategory), nameImages);

                    foreach (var file in listNameFiles)
                    {
                        pathImage.Add(new PathImage
                        {
                            SubCategory = subCategory,
                            PathNameImage = file.Key,
                            TypeImage = file.Value
                        });
                    }
                }
                else
                {
                    pathImage.Add(PathImageExtensions.GetDefaultPathNameFile(subCategory));
                }

                _context.AddRange(pathImage);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "TitleCategory", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories.Include(n => n.NameImages).FirstOrDefaultAsync(i => i.Id == id);

            foreach (var item in subCategory.NameImages)
            {
                if (item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                {
                    subCategory.NameImages.Remove(item);
                }
            }

            if (subCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "TitleCategory", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleSubCategory,CategoryId")] SubCategory subCategory, IFormFileCollection nameImages)
        {
            if (id != subCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subCategory);

                    if (nameImages.Count > 0)
                    {
                        var pathNameImages = await _context.PathImages.Where(i => i.SubCategoryId == subCategory.Id).ToListAsync();

                        foreach (var item in pathNameImages)
                        {
                            if (item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                            {
                                _context.Remove(item);
                            }
                        }

                        Dictionary<string, string> listNameFiles = await ChangeFiles.SaveCreateUploadFiles(subCategory.Id,
                            _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(subCategory), nameImages);

                        foreach (var nameFiles in listNameFiles)
                        {
                            _context.Add(new PathImage
                            {
                                SubCategory = subCategory,
                                PathNameImage = nameFiles.Key,
                                TypeImage = nameFiles.Value
                            });
                        }
                    }
                    else
                    {
                        var pathImagesDb = await _context.PathImages.Where(p => p.SubCategoryId == id).ToListAsync();

                        if (pathImagesDb.Count == 0)
                        {
                            _context.Add(PathImageExtensions.GetDefaultPathNameFile(subCategory));
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategory.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "TitleCategory", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            var nameImage = await _context.PathImages.Where(i => i.SubCategory == subCategory).Where(p => p.PathNameImage != "imagenot.jpg").ToListAsync();
            subCategory.NameImages = nameImage;
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _context.SubCategories.Include(n => n.NameImages).FirstOrDefaultAsync(i => i.Id == id);

            foreach (var item in subCategory.NameImages)
            {
                if (!item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                {
                    ChangeFiles.DeleteFiles(subCategory.Id, _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(subCategory), item.PathNameImage);
                }
            }

            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(int id)
        {
            return _context.SubCategories.Any(e => e.Id == id);
        }

        public async Task<VirtualFileResult> GetImage(int id)
        {
            if (id != null)
            {
                PathImage image = await _context.PathImages.FindAsync(id);
                if (image != null)
                {
                    string current = PathImageExtensions.GetDirectorySaveFile(image.SubCategory) + image.SubCategoryId;
                    return File(Path.Combine("~" + current, image.PathNameImage), image.TypeImage, image.PathNameImage);
                }

            }
            return null;
        }

        public async Task<IActionResult> DeleteImage(int id)
        {
            PathImage image = new PathImage();
            if (id != null)
            {
                image = await _context.PathImages.FindAsync(id);
                if (image != null)
                {
                    ChangeFiles.DeleteFiles(image.SubCategoryId.Value, _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(image.SubCategory), image.PathNameImage);

                    _context.PathImages.Remove(image);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Edit), new { id = image.SubCategoryId.Value });
        }
    }
}
