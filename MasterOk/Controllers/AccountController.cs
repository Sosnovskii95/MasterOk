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
                if (loginModel.InvateAdmin)
                {
                    User user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(
                                        l => l.EmailUser.Equals(loginModel.Email)
                                        && l.PasswordUser.Equals(loginModel.Password));

                    if (user != null)
                    {
                        await Authenticate(user.Id, user.Role.TitleRole);

                        return RedirectToAction(nameof(Index), "Users");
                    }
                    ModelState.AddModelError("", "Такого пользователя не существует! Проверьте данные для входа");

                    return View(loginModel);
                }
                else
                {
                    Client client = await _context.Clients.FirstOrDefaultAsync(
                    l => l.EmailClient.Equals(loginModel.Email)
                    && l.PasswordClient.Equals(loginModel.Password));

                    if (client != null)
                    {
                        await Authenticate(client.Id, "client");

                        return RedirectToAction(nameof(Index), "ClientPersonalArea");
                    }
                    ModelState.AddModelError("", "Такого клиента не существует! Проверьте данные для входа");

                    return View(loginModel);
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
                var email = await _context.Clients.Where(e => e.EmailClient.Equals(registerModel.EmailClient)).FirstOrDefaultAsync();

                if (email != null)
                {
                    ModelState.AddModelError("EmailClient", "Email занят");

                    return View(registerModel);
                }

                var numberPhone = await _context.Clients.Where(e => e.NumberPhone.Equals(registerModel.NumberPhone)).FirstOrDefaultAsync();

                if (numberPhone != null)
                {
                    ModelState.AddModelError("NumberPhone", "Номер телефона уже используется");

                    return View(registerModel);
                }

                Client client = new Client
                {
                    EmailClient = registerModel.EmailClient,
                    PasswordClient = registerModel.PasswordClient,
                    FirstLastNameClient = registerModel.FirstLastNameClient,
                    NumberPhone = registerModel.NumberPhone,
                    Address = registerModel.Address
                };

                _context.Add(client);
                await _context.SaveChangesAsync();

                await Authenticate(client.Id, "client");

                return RedirectToAction(nameof(Index), "ClientPersonalArea");
            }
            else
            {
                return View(registerModel);
            }
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(RegisterModel registerModel)
        {
            if(registerModel != null)
            {
                Client client = await _context.Clients.Where(n => n.EmailClient.Equals(registerModel.EmailClient) && n.NumberPhone.Equals(registerModel.NumberPhone)).FirstOrDefaultAsync();

                if(client != null)
                {
                    client.PasswordClient = client.EmailClient;

                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError("", "Некорректные данные");

            return View(registerModel);
        }

        public async Task<IActionResult> Logout()
        {
            var user = User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(Login));
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
