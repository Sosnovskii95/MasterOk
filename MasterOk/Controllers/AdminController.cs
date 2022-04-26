using Microsoft.AspNetCore.Mvc;

namespace MasterOk.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
