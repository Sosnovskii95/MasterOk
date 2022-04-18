﻿using Microsoft.AspNetCore.Mvc;
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
            List<CartClient> carts = new List<CartClient>();
            RegisterModel registerModel = new RegisterModel();

            ViewBag.PayMethod = await _context.PayMethods.ToListAsync();
            ViewBag.DeliveryMethod = await _context.DeliveryMethods.ToListAsync();

            var clientAunt = HttpContext.User.Identity;

            if (clientAunt is not null && clientAunt.IsAuthenticated)
            {
                var roleClient = HttpContext.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
                var idClient = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);

                if (roleClient.Value.Equals("client"))
                {
                    var client = await _context.Clients.FindAsync(Convert.ToInt32(idClient.Value));

                    if (client != null)
                    {
                        registerModel.Address = client.Address;
                        registerModel.FirstLastNameClient = client.FirstLastNameClient;
                        registerModel.EmailClient = client.EmailClient;
                        registerModel.NumberPhone = client.NumberPhone;

                        carts = await _context.CartClients.Include(p => p.Product).Where(i => i.ClientId == client.Id).ToListAsync();
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
            List<CartClient> cartClient;
            Client client = await GetClient(HttpContext);

            if(client != null)
            {
                cartClient = await _context.CartClients.Include(p => p.Product).Where(c => c.ClientId == client.Id).ToListAsync();
            }
            else
            {
                if (HttpContext.Session.Keys.Contains("cart"))
                {
                    cartClient = HttpContext.Session.Get<List<CartClient>>("cart");
                }
                else
                {
                    cartClient = new List<CartClient>();
                }
            }

            return PartialView(cartClient);
            /*
            List<CartClient> carts = new List<CartClient>();
            if (HttpContext.Session.Keys.Contains("cart"))
            {
                carts = HttpContext.Session.Get<List<CartClient>>("cart");
            }
            return PartialView(carts);*/
        }

        public async Task<IActionResult> AddProductCart(int id, int countCart)
        {
            Client client = await GetClient(HttpContext);
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
                        if (cartClient.Count() > 0)
                        {
                            //Добавляем его количество, изменяем стоимость на актуальную и общую стоимость
                            cartClient.Where(p => p.ProductId == product.Id).ToList().ForEach(f =>
                            {
                                f.CountCartProduct += countCart;
                                f.PriceCartProduct = product.Price;
                                f.TotalCartProduct = f.CountCartProduct * product.Price;
                            });
                            //сохраняем изменения в бд
                            _context.UpdateRange(cartClient);
                        }
                        else
                        {
                            //Если не существует, добавляем в корзину и в бл
                            _context.CartClients.Add(new CartClient
                            {
                                Product = product,
                                CountCartProduct = countCart,
                                PriceCartProduct = product.Price,
                                TotalCartProduct = product.Price * countCart,
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
                                f.CountCartProduct += countCart;
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
                                CountCartProduct = countCart,
                                PriceCartProduct = product.Price,
                                TotalCartProduct = countCart * product.Price
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

            /*if (id != null)
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    if (HttpContext.Session.Keys.Contains("cart"))
                    {
                        List<CartClient> cartClient = HttpContext.Session.Get<List<CartClient>>("cart");

                        if (cartClient.Where(p => p.Product.Id == product.Id).Count() > 0)
                        {
                            cartClient.Where(p => p.Product.Id == product.Id).ToList().ForEach(f =>
                            {
                                f.CountCartProduct += countCart;
                                f.PriceCartProduct = product.Price;
                                f.TotalCartProduct = f.CountCartProduct * product.Price;
                            });
                        }
                        else
                        {
                            cartClient.Add(new CartClient
                            {
                                Product = product,
                                CountCartProduct = countCart,
                                PriceCartProduct = product.Price,
                                TotalCartProduct = countCart * product.Price
                            });
                        }

                        HttpContext.Session.Set<List<CartClient>>("cart", cartClient);
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
            }*/
            //return null;
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
            //Если товар выбран хоть 1
            if (check.Count() > 0)
            {
                //проверям аунтетифицирован и авторизирован ли клиент
                Client client = await GetClient(HttpContext);
                bool aut = true;

                //если нет, то создаем его в бд и авторизируем
                if (client == null)
                {
                    client = new Client();
                    client.EmailClient = registerModel.EmailClient;
                    client.PasswordClient = registerModel.EmailClient;
                    client.NumberPhone = registerModel.NumberPhone;
                    client.FirstLastNameClient = registerModel.FirstLastNameClient;
                    client.Address = registerModel.Address;

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

                    //загружаем корзину в бд
                    foreach (var item in cartClient)
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
                    //получаем объекты выбранных товаров
                    cartClient = cartClient.Where(p => check.Contains(p.Product.Id)).ToList();
                }

                //если выбранный товар 1<
                if (cartClient.Count() > 0)
                {
                    //создаем чек
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

                    //создаем список проданных товаров
                    foreach (var item in cartClient)
                    {
                        _context.Add(new ProductSold
                        {
                            ProductId = item.Product.Id,
                            ProductCheckId = productCheck.Id,
                            CountSold = item.CountCartProduct
                        });
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect(HttpContext.Request.Headers.Referer);
            }

            /*Client clientAut = await GetClient(HttpContext);
            if (clientAut != null)
            {
                var cartClient = await _context.CartClients.Where(c => c.ClientId == clientAut.Id).Where(p => check.Contains(p.Product.Id)).ToListAsync();

                ProductCheck productCheck = new ProductCheck
                {
                    DateTimeSale = DateTime.Now,
                    StateOrder = "В обработке",
                    Client = clientAut,
                    DeliveryMethodId = deliveryMethodId,
                    PayMethodId = payMethodId
                };

                _context.ProductChecks.Add(productCheck);

                await _context.SaveChangesAsync();

                foreach (var item in cartClient)
                {
                    _context.Add(new ProductSold
                    {
                        ProductId = item.Product.Id,
                        ProductCheckId = productCheck.Id,
                        CountSold = item.CountCartProduct
                    });
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (check.Count() > 0)
                {
                    List<CartClient> cartClient = HttpContext.Session.Get<List<CartClient>>("cart");

                    var list = cartClient.Where(p => check.Contains(p.Product.Id)).ToList();

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

                        foreach (var item in cartClient)
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
            }*/

            //return Redirect(HttpContext.Request.Headers.Referer);
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

        private async Task<Client> GetClient(HttpContext httpContext)
        {
            var clientAut = httpContext.User.Identity;

            if (clientAut is not null && clientAut.IsAuthenticated)
            {
                var roleClient = httpContext.User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType);
                var idClient = httpContext.User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);

                if (roleClient.Equals("client"))
                {
                    return await _context.Clients.FindAsync(Convert.ToInt32(idClient));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
