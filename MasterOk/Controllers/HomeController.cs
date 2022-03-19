using MasterOk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MasterOk.Data;
using Microsoft.EntityFrameworkCore;

namespace MasterOk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;

        public HomeController(ILogger<HomeController> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Test(IFormFileCollection fileName)
        {
            return null;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> ShowSubCategories(int categoriesId)
        {
            if(categoriesId > 0)
            {
                return View(await _context.SubCategories.Where(i=>i.CategoryId == categoriesId).ToListAsync());
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
                return View(await _context.Products.Where(i => i.SubCategoryId == subCategoriesId).ToListAsync());
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