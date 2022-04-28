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
            List<CartClient> cartClient;
            RegisterModel registerModel = new RegisterModel();

            ViewBag.PayMethod = await _context.PayMethods.ToListAsync();
            ViewBag.DeliveryMethod = await _context.DeliveryMethods.ToListAsync();

            Client client = await GetAuthenticateClient(HttpContext);

            if (client != null)
            {
                registerModel.Address = client.Address;
                registerModel.FirstLastNameClient = client.FirstLastNameClient;
                registerModel.EmailClient = client.EmailClient;
                registerModel.NumberPhone = client.NumberPhone;

                cartClient = await _context.CartClients.Include(p => p.Product).Where(i => i.ClientId == client.Id).ToListAsync();
                cartClient.ForEach(f =>
                {
                    f.PriceCartProduct = f.Product.Price;
                    f.TotalCartProduct = f.Product.Price * f.CountCartProduct;
                });
            }
            else
            {
                if (HttpContext.Session.Keys.Contains("cart"))
                    cartClient = HttpContext.Session.Get<List<CartClient>>("cart");
                else
                    cartClient = new List<CartClient>();
            }

            return View(new CartRegisterModel { CartClients = cartClient, RegisterModel = registerModel });
        }

        public async Task<PartialViewResult> IndexPartital()
        {
            List<CartClient> cartClient;
            Client client = await GetAuthenticateClient(HttpContext);

            if (client != null)
            {
                cartClient = await _context.CartClients.Include(p => p.Product).Where(c => c.ClientId == client.Id).ToListAsync();
            }
            else
            {
                if (HttpContext.Session.Keys.Contains("cart"))
                    cartClient = HttpContext.Session.Get<List<CartClient>>("cart");
                else
                    cartClient = new List<CartClient>();

            }

            return PartialView(cartClient);
        }

        public async Task<IActionResult> AddProductCart(int id)
        {
            Client client = await GetAuthenticateClient(HttpContext);
            List<CartClient> cartClient;

            //Если id не null
            if (id != null)
            {
                //Запрос в базу товар
                Product product = await _context.Products.FindAsync(id);

                //Если товар существует
                if (product != null)
                {
                    //Проверяем авторизацию клиента
                    if (client != null)
                    {
                        //Получаем корзину клиента из бд с заданным товаром
                        cartClient = await _context.CartClients.Where(c => c.ClientId == client.Id).Where(p => p.ProductId == product.Id).ToListAsync();

                        //Если товар есть в корзине
                        if (cartClient.Count > 0)
                        {
                            //Добавляем его количество, изменяем стоимость на актуальную и общую стоимость
                            cartClient.Where(p => p.ProductId == product.Id).ToList().ForEach(f =>
                            {
                                f.CountCartProduct += 1;
                                f.PriceCartProduct = product.Price;
                                f.TotalCartProduct = f.CountCartProduct * product.Price;
                            });
                            //сохраняем изменения в бд
                            _context.UpdateRange(cartClient);
                        }
                        else
                        {
                            //Если не существует, добавляем в корзину и в бл
                            _context.Add(new CartClient
                            {
                                Product = product,
                                CountCartProduct = 1,
                                PriceCartProduct = product.Price,
                                TotalCartProduct = product.Price * 1,
                                Client = client
                            });
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //Если клиент не авторизирован работаем с сессией
                        //Получаем текущее состояние сессии
                        cartClient = HttpContext.Session.Get<List<CartClient>>("cart");

                        //Если корзина существует в сессии, и существует товар в ней заданный
                        if (cartClient != null && cartClient.Where(p => p.ProductId == product.Id).ToList().Count() > 0)
                        {
                            //изменяем его состояние
                            cartClient.Where(p => p.ProductId == product.Id).ToList().ForEach(f =>
                            {
                                f.CountCartProduct += 1;
                                f.PriceCartProduct = product.Price;
                                f.TotalCartProduct = f.CountCartProduct * product.Price;
                            });
                        }
                        else
                        {
                            //Если корзина пуста, то создаем ее
                            if (cartClient == null)
                                cartClient = new List<CartClient>();

                            //Добавлем товар либо в существующую корзину, либо созданную выше
                            cartClient.Add(new CartClient
                            {
                                Product = product,
                                CountCartProduct = 1,
                                PriceCartProduct = product.Price,
                                TotalCartProduct = 1 * product.Price
                            });
                        }
                        //Сетим значение корзины в сессию
                        HttpContext.Session.Set<List<CartClient>>("cart", cartClient);
                    }
                }
                else
                {
                    //Выброс ошибки
                    return NotFound();
                }

                //Переадрессовываем на место вызова
                return Redirect(HttpContext.Request.Headers.Referer);
            }
            else
            {
                //Выброс ошибки
                return NotFound();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            Client client = await GetAuthenticateClient(HttpContext);
            List<CartClient> cartClient;

            if (id != null)
            {
                if (client != null)
                {
                    cartClient = await _context.CartClients.Where(c => c.ClientId == client.Id).Where(p => p.Product.Id == id).ToListAsync();

                    if (cartClient.Count > 0)
                    {
                        _context.RemoveRange(cartClient);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    cartClient = HttpContext.Session.Get<List<CartClient>>("cart");

                    if (cartClient.Count > 0)
                    {
                        cartClient.RemoveAll(p => p.Product.Id == id);
                    }

                    HttpContext.Session.Set<List<CartClient>>("cart", cartClient);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect(HttpContext.Request.Headers.Referer);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Change(int id, int valueId)
        {
            Client client = await GetAuthenticateClient(HttpContext);
            List<CartClient> cartClient;

            if (id != null)
            {
                Product product = await _context.Products.FindAsync(id);

                if (product != null)
                {
                    valueId = valueId <= product.CountStoreProduct ? valueId : product.CountStoreProduct;

                    if (client != null)
                    {
                        cartClient = await _context.CartClients.Where(c => c.ClientId == client.Id).Where(p => p.ProductId == id).Include(p => p.Product).ToListAsync();
                        cartClient.ForEach(f =>
                        {
                            f.CountCartProduct = valueId;
                            f.PriceCartProduct = f.Product.Price;
                            f.TotalCartProduct = valueId * f.PriceCartProduct;
                        });

                        _context.UpdateRange(cartClient);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        cartClient = HttpContext.Session.Get<List<CartClient>>("cart");

                        cartClient.Where(p => p.Product.Id == id).ToList().ForEach(f =>
                        {
                            f.CountCartProduct = valueId;
                            f.PriceCartProduct = f.Product.Price;
                            f.TotalCartProduct = valueId * f.PriceCartProduct;
                        });

                        HttpContext.Session.Set<List<CartClient>>("cart", cartClient);
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(List<int> check, RegisterModel registerModel, int payMethodId, int deliveryMethodId)
        {
            //Если товар выбран хоть 1
            if (check.Count > 0)
            {
                //проверям аунтетифицирован и авторизирован ли клиент
                Client client = await GetAuthenticateClient(HttpContext);
                bool aut = true;

                //если нет, то создаем его в бд и авторизируем
                if (client == null)
                {
                    client = new Client
                    {
                        EmailClient = registerModel.EmailClient,
                        PasswordClient = registerModel.EmailClient,
                        NumberPhone = registerModel.NumberPhone,
                        FirstLastNameClient = registerModel.FirstLastNameClient,
                        Address = registerModel.Address
                    };

                    aut = false;

                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();

                    await Authenticate(client.Id, "client");
                }

                List<CartClient> cartClient;

                //получаем корзину клиента, если авторизирован то из бд
                if (aut)
                {
                    //получаем сразу выбранные товары из бд
                    cartClient = await _context.CartClients.Where(c => c.ClientId == client.Id).Where(p => check.Contains(p.Product.Id)).Include(p => p.Product).ToListAsync();
                }
                else
                {
                    //если нет, то из текущей сессии
                    cartClient = HttpContext.Session.Get<List<CartClient>>("cart");

                    List<CartClient> removeProductnotFound = new List<CartClient>();
                    //загружаем корзину в бд
                    foreach (var item in cartClient)
                    {
                        //проверяем актуальность товаров из сессии с базой
                        Product product = await _context.Products.FindAsync(item.Product.Id);

                        //если товар существует в бд
                        if (product != null)
                        {
                            //добавлем его в корзину
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
                        else
                        {
                            //если нет создаем список с неактулальными позициями
                            removeProductnotFound.Add(item);
                        }
                    }
                    //удаляем неактуальные товары из корзины сессии
                    foreach (var item in removeProductnotFound)
                    {
                        cartClient.Remove(item);
                    }

                    //получаем объекты выбранных товаров
                    cartClient = cartClient.Where(p => check.Contains(p.Product.Id)).ToList();
                }

                //если выбранный товар 1<
                if (cartClient.Count > 0)
                {
                    //создаем чек
                    ProductCheck productCheck = new ProductCheck
                    {
                        DateTimeSale = DateTime.Now,
                        StateOrderId = 1,
                        Client = client,
                        DeliveryMethodId = deliveryMethodId,
                        PayMethodId = payMethodId
                    };

                    _context.ProductChecks.Add(productCheck);
                    await _context.SaveChangesAsync();

                    //создаем список проданных товаров
                    foreach (var item in cartClient)
                    {
                        _context.Add(new ProductSold
                        {
                            ProductId = item.Product.Id,
                            ProductCheckId = productCheck.Id,
                            CountSold = item.CountCartProduct,
                            PriceSold = item.PriceCartProduct,
                            TotalSold = item.CountCartProduct * item.PriceCartProduct
                        });

                        Product product = await _context.Products.FirstOrDefaultAsync(i => i.Id == item.Product.Id);

                        product.CountStoreProduct -= item.CountCartProduct;

                        _context.Update(product);

                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect(HttpContext.Request.Headers.Referer);
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
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie");
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
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
