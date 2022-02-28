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
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                Client client = await _context.Clients.FirstOrDefaultAsync(
                    l => l.LoginClient.Equals(loginModel.LoginEmail) 
                    || l.EmailClient.Equals(loginModel.LoginEmail)
                    && l.PasswordClient.Equals(loginModel.Password));

                if(client != null)
                {
                    Authenticate(client.Id, "client");
                }
                else
                {
                    User user = await _context.Users.Include(r=>r.Role).FirstOrDefaultAsync(
                    l => l.LoginUser.Equals(loginModel.LoginEmail)
                    && l.PasswordUser.Equals(loginModel.Password));

                    if(user != null)
                    {
                        Authenticate(user.Id, user.Role.TitleRole);
                    }
                }
            }

            return null;
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
                    LoginClient = registerModel.LoginClient,
                    PasswordClient = registerModel.PasswordClient,
                    FirstNameClient = registerModel.FirstNameClient,
                    LastNameClient = registerModel.LastNameClient
                });

                await _context.SaveChangesAsync();
            }
            else
            {
                return View(registerModel);
            }
            return null;
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
