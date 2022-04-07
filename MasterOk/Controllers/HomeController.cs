using MasterOk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MasterOk.Data;
using Microsoft.EntityFrameworkCore;
using MasterOk.Models.ModelDataBase;
using MasterOk.Models.Serealize;

namespace MasterOk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _webHost;

        public HomeController(ILogger<HomeController> logger, DataBaseContext context, IWebHostEnvironment webHost)
        {
            _logger = logger;
            _context = context;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.Include(p => p.NameImages).ToListAsync());
        }

        public async Task<VirtualFileResult> GetImage(int id, string typeObject)
        {
            PathImage image = null;
            string currentDirectory = "";

            if (id != null)
            {
                image = await _context.PathImages.FindAsync(id);
                if (image != null)
                {
                    if (!image.PathNameImage.Equals(PathImageExtensions.GetPathNameImage()))
                    {
                        if (!String.IsNullOrEmpty(typeObject))
                        {
                            switch (typeObject)
                            {
                                case "category":
                                    currentDirectory = PathImageExtensions.GetDirectorySaveFile(image.Category) + image.CategoryId;
                                    break;
                                case "subCategory":
                                    currentDirectory = PathImageExtensions.GetDirectorySaveFile(image.SubCategory) + image.SubCategoryId;
                                    break;
                                case "product":
                                    currentDirectory = PathImageExtensions.GetDirectorySaveFile(image.Product) + image.ProductId;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        currentDirectory = PathImageExtensions.GetDirectoryFile();
                        image.PathNameImage = PathImageExtensions.GetPathNameImage();
                        image.TypeImage = PathImageExtensions.GetTypeImage();
                    }
                }
            }
            if (CheckFile(_webHost.WebRootPath + currentDirectory, image.PathNameImage))
            {
                return File(Path.Combine("~" + currentDirectory, image.PathNameImage), image.TypeImage, image.PathNameImage);
            }
            else
            {
                return File(Path.Combine("~" + currentDirectory, image.PathNameImage), image.TypeImage, image.PathNameImage);
            }
        }

        private bool CheckFile(string currentDirectory, string fileName)
        {
            bool result = true;


            if (!(System.IO.File.Exists(Path.Combine(currentDirectory, fileName))))
            {
                result = false;
            }

            return result;
        }


        public async Task<IActionResult> ShowSubCategories(int categoriesId)
        {
            if (categoriesId > 0)
            {
                return View(await _context.SubCategories.Where(i => i.CategoryId == categoriesId).Include(p => p.NameImages).ToListAsync());
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ShowProducts(int subCategoriesId)
        {
            if (subCategoriesId > 0)
            {
                return View(await _context.Products.Where(i => i.SubCategoryId == subCategoriesId).Include(p => p.NameImages).ToListAsync());
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Product(int productId)
        {
            if (productId > 0)
            {
                return View(await _context.Products.FindAsync(productId));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}