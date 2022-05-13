using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;

namespace MasterOk.Controllers
{
    [Authorize(Roles = "user")]
    public class AdminController : Controller
    {
        private readonly DataBaseContext _context;

        public AdminController(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));

            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
