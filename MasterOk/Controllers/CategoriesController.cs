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

                Dictionary<string, string> listNameFiles = await ChangeFiles.SaveCreateUploadFiles(category.Id, _webHost.WebRootPath + "/Content/Category", nameImages);

                foreach (var nameFiles in listNameFiles)
                {
                    _context.Add(new PathImage
                    {
                        CategoryId = category.Id,
                        PathNameImage = nameFiles.Key,
                        TypeImage = nameFiles.Value
                    });
                }

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
                    await _context.SaveChangesAsync();

                    Dictionary<string, string> listNameFiles = await ChangeFiles.SaveCreateUploadFiles(category.Id, _webHost.WebRootPath + "/Content/Category", nameImages);

                    foreach (var nameFiles in listNameFiles)
                    {
                        _context.Add(new PathImage
                        {
                            CategoryId = category.Id,
                            PathNameImage = nameFiles.Key,
                            TypeImage = nameFiles.Value
                        });
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
            var category = await _context.Categories.FindAsync(id);
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
                    string current = "/Content/Category/" + image.CategoryId;
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
                    ChangeFiles.DeleteFiles(image.CategoryId.Value, _webHost.WebRootPath + "/Content/Category/", image.PathNameImage);

                    _context.PathImages.Remove(image);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Edit), new { id = image.CategoryId.Value });
        }
    }
}
