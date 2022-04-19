using MasterOk.Models.ModelAuthorization;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MasterOk.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataBaseContext _context;

        public AccountController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string ? returnUrl)
        {
            if (ModelState.IsValid)
            {
                Client client = await _context.Clients.FirstOrDefaultAsync(
                    l => l.EmailClient.Equals(loginModel.LoginEmail)
                    && l.PasswordClient.Equals(loginModel.Password));

                if (client != null)
                {
                    await Authenticate(client.Id, "client");

                    return RedirectToAction(nameof(Index), nameof(ClientsController));
                }

                User user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(
                    l => l.LoginUser.Equals(loginModel.LoginEmail)
                    && l.PasswordUser.Equals(loginModel.Password));

                if(user != null)
                {
                    await Authenticate(user.Id, user.Role.TitleRole);

                    return RedirectToAction(nameof(Index), nameof(UsersController));
                }
            }

            return View(loginModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                await _context.Clients.AddAsync(new Client
                {
                    EmailClient = registerModel.EmailClient,
                    PasswordClient = registerModel.PasswordClient,
                    FirstLastNameClient = registerModel.FirstLastNameClient,
                    NumberPhone = registerModel.NumberPhone,
                    Address = registerModel.Address
                });

                await _context.SaveChangesAsync();

                return Redirect(HttpContext.Request.Headers.Referer);
            }
            else
            {
                return View(registerModel);
            }
        }

        private async Task Authenticate(int idClientUser, string titleRole)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, idClientUser.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, titleRole)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
