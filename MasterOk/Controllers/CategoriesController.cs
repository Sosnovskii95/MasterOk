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
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    //[Authorize(Roles = "user")]
    public class CategoriesController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _webHost;

        public CategoriesController(DataBaseContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            var nameImage = await _context.PathImages.Where(i => i.Category == category).Where(m => m.PathNameImage != "imagenot.jpg").ToListAsync();
            category.NameImages = nameImage;
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleCategory")] Category category, IFormFileCollection nameImages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();

                List<PathImage> pathImage = new List<PathImage>();

                if (nameImages.Count > 0)
                {
                    Dictionary<string, string> listNameFiles = await ChangeFiles.SaveCreateUploadFiles(category.Id,
                        _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(category), nameImages);

                    foreach (var nameFiles in listNameFiles)
                    {
                        pathImage.Add(new PathImage
                        {
                            Category = category,
                            PathNameImage = nameFiles.Key,
                            TypeImage = nameFiles.Value
                        });
                    }
                }
                else
                {
                    pathImage.Add(PathImageExtensions.GetDefaultPathNameFile(category));
                }

                _context.AddRange(pathImage);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.Include(p => p.NameImages).FirstOrDefaultAsync(i => i.Id == id);

            foreach (var item in category.NameImages)
            {
                if (item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                {
                    category.NameImages.Remove(item);
                }
            }

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleCategory")] Category category, IFormFileCollection nameImages)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);

                    if (nameImages.Count > 0)
                    {
                        var pathNameImages = await _context.PathImages.Where(i => i.CategoryId == category.Id).ToListAsync();

                        foreach (var item in pathNameImages)
                        {
                            if (item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                            {
                                _context.Remove(item);
                            }
                        }

                        Dictionary<string, string> listNameFiles = await ChangeFiles.SaveCreateUploadFiles(category.Id,
                            _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(category), nameImages);

                        foreach (var nameFiles in listNameFiles)
                        {
                            _context.Add(new PathImage
                            {
                                Category = category,
                                PathNameImage = nameFiles.Key,
                                TypeImage = nameFiles.Value
                            });
                        }
                    }
                    else
                    {
                        var pathImagesDb = await _context.PathImages.Where(p => p.CategoryId == id).ToListAsync();

                        if (pathImagesDb.Count == 0)
                        {
                            _context.Add(PathImageExtensions.GetDefaultPathNameFile(category));
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            var nameImage = await _context.PathImages.Where(i => i.Category == category).Where(m => m.PathNameImage != "imagenot.jpg").ToListAsync();
            category.NameImages = nameImage;
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.Include(p => p.NameImages).FirstOrDefaultAsync(i => i.Id == id);

            foreach (var item in category.NameImages)
            {
                if (!item.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                {
                    ChangeFiles.DeleteFiles(category.Id, _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(category), item.PathNameImage);
                }
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public async Task<VirtualFileResult> GetImage(int id)
        {
            if (id != null)
            {
                PathImage image = await _context.PathImages.FindAsync(id);
                if (image != null)
                {
                    string current = PathImageExtensions.GetDirectorySaveFile(image.Category) + image.CategoryId;
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
                    ChangeFiles.DeleteFiles(image.CategoryId.Value, _webHost.WebRootPath + PathImageExtensions.GetDirectorySaveFile(image.Category), image.PathNameImage);

                    _context.PathImages.Remove(image);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Edit), new { id = image.CategoryId.Value });
        }
    }
}
