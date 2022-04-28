using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        [Authorize(Roles = "client")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> Edit()
        {
            Client client = await GetAuthenticateClient(HttpContext);

            if (client != null)
            {
                return View(client);
            }
            else
            {
                return Redirect(HttpContext.Request.Headers.Referer);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "client")]
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
            Client client = await GetAuthenticateClient(HttpContext);
            List<ProductCheck> productChecks = new List<ProductCheck>();

            if (client != null)
            {
                productChecks = await _context.ProductChecks.Where(i => i.ClientId == client.Id).
                                                             Include(p => p.ProductSolds).
                                                             Include(p => p.PayMethod).
                                                             Include(d => d.DeliveryMethod).
                                                             Include(s => s.StateOrder).ToListAsync();
                return View(productChecks);
            }
            else
            {
                return View(productChecks);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id != null)
            {
                return View(await _context.ProductSolds.Where(p => p.ProductCheckId == id).Include(o => o.Product).ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }

        private async Task<Client> GetAuthenticateClient(HttpContext httpContext)
        {
            var clientAut = httpContext.User.Identity;

            if (clientAut is not null && clientAut.IsAuthenticated)
            {
                var roleClient = httpContext.User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType);
                var idClient = httpContext.User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);

                if (roleClient.Equals("client"))
                {
                    Client client = await _context.Clients.FindAsync(Convert.ToInt32(idClient));

                    if (client == null)
                    {
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                    else
                    {
                        return client;
                    }
                }
            }

            return null;
        }
    }
}
