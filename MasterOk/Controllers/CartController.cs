using Microsoft.AspNetCore.Mvc;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using MasterOk.Models.ModelAuthorization;
using MasterOk.Models.Serealize;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace MasterOk.Controllers
{
    public class CartController : Controller
    {
        private readonly DataBaseContext _context;

        public CartController(DataBaseContext context)
        {
            _context = context;
        }

        //  [Authorize(Roles = "client")]
        public async Task<IActionResult> Index()
        {
            /*List<CartClient> carts = new List<CartClient>();

            if (HttpContext.Session.Keys.Contains("cart"))
            {
                carts = HttpContext.Session.Get<List<CartClient>>("cart");
            }

            ViewBag.PayMethod = await _context.PayMethods.ToListAsync();
            ViewBag.DeliveryMethod = await _context.DeliveryMethods.ToListAsync();

            var clientAut = HttpContext.User.Identity;

            RegisterModel registerModel = new RegisterModel();

            if(clientAut is not null && clientAut.IsAuthenticated)
            {
                var role = HttpContext.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
                var clientId = HttpContext.User.FindFirst(ClaimTypes.Name);

                if (role.Value.Equals("client"))
                {
                    var client = await _context.Clients.FindAsync(Convert.ToInt32(clientId.Value));

                    if(client != null)
                    {
                        registerModel.Address = client.Address;
                        registerModel.FirstLastNameClient = client.FirstLastNameClient;
                        registerModel.EmailClient = client.EmailClient;
                        registerModel.NumberPhone = client.NumberPhone;
                    }
                }
            }*/

            List<CartClient> carts = new List<CartClient>();
            RegisterModel registerModel = new RegisterModel();

            ViewBag.PayMethod = await _context.PayMethods.ToListAsync();
            ViewBag.DeliveryMethod = await _context.DeliveryMethods.ToListAsync();

            var clientAunt = HttpContext.User.Identity;

            if(clientAunt is not null && clientAunt.IsAuthenticated)
            {
                var roleClient = HttpContext.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
                var idClient = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);

                if (roleClient.Value.Equals("client"))
                {
                    var client = await _context.Clients.FindAsync(Convert.ToInt32(idClient.Value));

                    if(client != null)
                    {
                        registerModel.Address = client.Address;
                        registerModel.FirstLastNameClient = client.FirstLastNameClient;
                        registerModel.EmailClient = client.EmailClient;
                        registerModel.NumberPhone = client.NumberPhone;

                        carts = await _context.CartClients.Include(p=>p.Product).Where(i => i.ClientId == client.Id).ToListAsync();
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (HttpContext.Session.Keys.Contains("cart"))
                {
                    carts = HttpContext.Session.Get<List<CartClient>>("cart");
                }
            }

            return View(new CartRegisterModel { CartClients = carts, RegisterModel = registerModel });
        }

        public async Task<PartialViewResult> IndexPartital()
        {
            List<CartClient> carts = new List<CartClient>();
            if (HttpContext.Session.Keys.Contains("cart"))
            {
                carts = HttpContext.Session.Get<List<CartClient>>("cart");
            }
            return PartialView(carts);
        }

        public async Task<IActionResult> AddProductCart(int id, int countCart)
        {
            if (id != null)
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    if (HttpContext.Session.Keys.Contains("cart"))
                    {
                        List<CartClient> cartClients = HttpContext.Session.Get<List<CartClient>>("cart");

                        if (cartClients.Where(p => p.Product.Id == product.Id).Count() > 0)
                        {
                            cartClients.Where(p => p.Product.Id == product.Id).ToList().ForEach(f =>
                            {
                                f.CountCartProduct += countCart;
                                f.PriceCartProduct = product.Price;
                                f.TotalCartProduct = f.CountCartProduct * product.Price;
                            });
                        }
                        else
                        {
                            cartClients.Add(new CartClient
                            {
                                Product = product,
                                CountCartProduct = countCart,
                                PriceCartProduct = product.Price,
                                TotalCartProduct = countCart * product.Price
                            });
                        }

                        HttpContext.Session.Set<List<CartClient>>("cart", cartClients);
                    }
                    else
                    {
                        HttpContext.Session.Set<List<CartClient>>("cart", new List<CartClient> {
                            new CartClient {
                                Product = product,
                                CountCartProduct = countCart,
                                PriceCartProduct = product.Price,
                                TotalCartProduct = countCart * product.Price
                            }
                        });
                    }
                }
                return Redirect(HttpContext.Request.Headers.Referer);
            }
            return null;
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id != null)
            {
                if (HttpContext.Session.Keys.Contains("cart"))
                {
                    List<CartClient> cartClients = HttpContext.Session.Get<List<CartClient>>("cart");

                    if (cartClients.Count() > 0)
                    {
                        cartClients.RemoveAll(p => p.Product.Id == id);
                    }

                    HttpContext.Session.Set<List<CartClient>>("cart", cartClients);

                    return RedirectToAction(nameof(Index));
                }
            }

            return Redirect(HttpContext.Request.Headers.Referer);
        }

        [HttpPost]
        public IActionResult Change(int id, int valueId)
        {
            if (id != null)
            {
                if (HttpContext.Session.Keys.Contains("cart"))
                {
                    List<CartClient> cartClients = HttpContext.Session.Get<List<CartClient>>("cart");

                    if (cartClients.Where(c => c.Product.Id == id).Count() > 0)
                    {
                        cartClients.Where(c => c.Product.Id == id).ToList().ForEach(f =>
                        {
                            f.CountCartProduct = valueId;
                            f.TotalCartProduct = valueId * f.PriceCartProduct;
                        });
                    }

                    HttpContext.Session.Set<List<CartClient>>("cart", cartClients);

                    return RedirectToAction(nameof(Index));
                }
            }
            return Redirect(HttpContext.Request.Headers.Referer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(List<int> check, RegisterModel registerModel, int payMethodId, int deliveryMethodId)
        {
            if (check.Count() > 0)
            {
                List<CartClient> cartClients = HttpContext.Session.Get<List<CartClient>>("cart");

                var list = cartClients.Where(p => check.Contains(p.Product.Id)).ToList();

                if (list.Count() > 0)
                {
                    Client client = new Client
                    {
                        EmailClient = registerModel.EmailClient,
                        PasswordClient = registerModel.EmailClient,
                        NumberPhone = registerModel.NumberPhone,
                        FirstLastNameClient = registerModel.FirstLastNameClient,
                        Address = registerModel.Address
                    };

                    _context.Clients.Add(client);

                    ProductCheck productCheck = new ProductCheck
                    {
                        DateTimeSale = DateTime.Now,
                        StateOrder = "В обработке",
                        Client = client,
                        DeliveryMethodId = deliveryMethodId,
                        PayMethodId = payMethodId
                    };

                    _context.ProductChecks.Add(productCheck);

                    await _context.SaveChangesAsync();

                    foreach (var item in list)
                    {
                        _context.Add(new ProductSold
                        {
                            ProductId = item.Product.Id,
                            ProductCheckId = productCheck.Id,
                            CountSold = item.CountCartProduct
                        });
                        await _context.SaveChangesAsync();
                    }

                    foreach(var item in cartClients)
                    {
                        _context.Add(new CartClient
                        {
                            ClientId = client.Id,
                            PriceCartProduct = item.Product.Price,
                            CountCartProduct = item.CountCartProduct,
                            ProductId = item.Product.Id,
                            TotalCartProduct = item.TotalCartProduct
                        });
                        await _context.SaveChangesAsync();
                    }

                    await Authenticate(client.Id, "client");

                    return RedirectToAction(nameof(Index));
                }
                //Нужно сделать авторизацию и регистрацию дабы не падало с ошибкой
            }

            return Redirect(HttpContext.Request.Headers.Referer);
        }

        public async Task<IActionResult> Get()
        {
            var client = HttpContext.User.Identity;

            var clientId = HttpContext.User.FindFirst(ClaimTypes.Name);

            await Authenticate(1, "client");
            return Redirect(nameof(Index));
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
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie");
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
