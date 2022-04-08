using Microsoft.AspNetCore.Mvc;
using MasterOk.Data;
using MasterOk.Models.ModelDataBase;
using MasterOk.Models.Serealize;

namespace MasterOk.Controllers
{
    public class CartController : Controller
    {
        private readonly DataBaseContext _context;

        public CartController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<CartClient> carts = new List<CartClient>();

            if (HttpContext.Session.Keys.Contains("cart"))
            {
                carts = HttpContext.Session.Get<List<CartClient>>("cart");
            }

            return View(carts);
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
                            cartClients.Where(p => p.Product.Id == product.Id).ToList().ForEach(f => { f.CountCartProduct += countCart; f.PriceCartProduct = product.Price; f.TotalCartProduct = f.CountCartProduct * product.Price; });
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
                        cartClients.Where(c => c.Product.Id == id).ToList().ForEach(f => { f.CountCartProduct = valueId; f.TotalCartProduct = valueId * f.PriceCartProduct; });
                    }

                    HttpContext.Session.Set<List<CartClient>>("cart", cartClients);

                    return RedirectToAction(nameof(Index));
                }
            }
            return Redirect(HttpContext.Request.Headers.Referer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(List<int> check)
        {
            if (check.Count() > 0)
            {
                List<CartClient> cartClients = HttpContext.Session.Get<List<CartClient>>("cart");

                var list = cartClients.Where(p => check.Contains(p.Product.Id)).ToList();

                if (list.Count() > 0)
                {
                    ProductCheck productCheck = new ProductCheck
                    {
                        DateTimeSale = DateTime.Now,
                        StateOrder = "В обработке"
                    };

                    _context.ProductChecks.Add(productCheck);

                    foreach(var item in list)
                    {
                        _context.Add(new ProductSold
                        {
                            Product = await _context.Products.FindAsync(item.Product.Id),
                            ProductCheck = productCheck,
                            CountSold = item.CountCartProduct
                        });
                        await _context.SaveChangesAsync();
                    }
                }
                //Нужно сделать авторизацию и регистрацию дабы не падало с ошибкой
            }

            return null;
        }
    }
}
