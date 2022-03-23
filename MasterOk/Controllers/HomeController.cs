using MasterOk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MasterOk.Data;
using Microsoft.EntityFrameworkCore;
using MasterOk.Models.ModelDataBase;

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
                    if (!String.IsNullOrEmpty(typeObject))
                    {
                        switch (typeObject)
                        {
                            case "category":
                                currentDirectory = "/Content/Category/" + image.CategoryId;
                                break;
                            case "subCategory":
                                currentDirectory = "/Content/SubCategory/" + image.SubCategoryId;
                                break;
                            case "product":
                                currentDirectory = "/Content/Product" + image.ProductId;
                                break;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            if (CheckFile(_webHost.WebRootPath + currentDirectory, image.PathNameImage))
            {
                return File(Path.Combine("~" + currentDirectory, image.PathNameImage), image.TypeImage, image.PathNameImage);
            }
            else
            {
                return null;
            }
            /*string currentDirectory = "";
            PathImage image = null;

            if (!String.IsNullOrEmpty(typeObject))
            {
                if (typeObject.Equals("product"))
                {
                    image = await _context.PathImages.FirstOrDefaultAsync(i => i.ProductId == id);
                    currentDirectory += "/Content/Product/" + id;
                }
                else if (typeObject.Equals("subCategory"))
                {
                    image = await _context.PathImages.FirstOrDefaultAsync(i => i.SubCategoryId == id);
                    currentDirectory += "/Content/SubCategory/" + id;
                }
                else if (typeObject.Equals("category"))
                {
                    image = await _context.PathImages.FirstOrDefaultAsync(i => i.CategoryId == id);
                    currentDirectory += "/Content/Category/" + id;
                }
                else if (typeObject.Equals("slider"))
                {
                    image = await _context.PathImages.FindAsync(id);
                    currentDirectory += "/Content/Slider/" + image.Id;
                }
                else if (typeObject.Equals("maps"))
                {
                    image = new PathImage { NameImage = "maps.png", TypeImage = "image/png" };
                    currentDirectory = "/Content/";
                }
            }
            if (CheckFile(_webHost.WebRootPath + currentDirectory, image.PathNameImage))
            {
                return File(Path.Combine("~" + currentDirectory, image.PathNameImage), image.TypeImage, image.PathNameImage);
            }
            else
            {
                currentDirectory = "~/Content/";
                image = new PathImage { NameImage = "image-aborted.jpg" };
                return File(Path.Combine(currentDirectory, image.NameImage), "image/jpg", image.NameImage);
            }*/
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
                return View(await _context.Products.Where(i => i.SubCategoryId == subCategoriesId).Include(p=>p.NameImages).ToListAsync());
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