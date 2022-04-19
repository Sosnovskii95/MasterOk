using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MasterOk.Controllers
{
    public class ClientsController : Controller
    {
        private readonly DataBaseContext _context;

        public ClientsController(DataBaseContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "client")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> Edit()
        {
            int clientId = Convert.ToInt32(User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);

            Client client = await _context.Clients.FindAsync(clientId);
            if (client != null)
            {
                return View(client);
            }

            return Redirect(HttpContext.Request.Headers.Referer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "client")]
        public async Task<IActionResult> Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(client);
            }
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> History()
        {
            int clientId = Convert.ToInt32(User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);

            IQueryable<ProductCheck> productChecks = _context.ProductChecks.Where(i => i.ClientId == clientId);

            if (productChecks != null)
            {
                return View(productChecks.ToList());
            }
            else
            {
                return null;
            }
        }

        //[Authorize(Roles = "client")]
        public async Task<IActionResult> Cart()
        {
            int clientId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);

            Client client = await _context.Clients.Include(c => c.CartClients).ThenInclude(p => p.Product).FirstOrDefaultAsync(i => i.Id == clientId);

            if (client != null)
            {
                return View(client);
            }
            else
            {
                return null;
            }
        }
    }
}
