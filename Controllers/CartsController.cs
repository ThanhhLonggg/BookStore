using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookStore.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext db;
       
        //add cart
        public IActionResult addCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart == null)
            {
                var book = getDetailBook(id);
                List<Cart> listCart = new List<Cart>()
                {
                    new Cart
                    {
                        Book= book,
                        Quantity = 1
                    }
                };
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));
            }
            else
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                bool check = true;
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Book.Id == id)
                    {
                        dataCart[i].Quantity++;
                        check = false;
                    }
                    if (check)
                    {
                        dataCart.Add(new Cart
                        {
                            Book = getDetailBook(id),
                            Quantity = 1
                        });
                    }
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
            }
            return RedirectToAction(nameof(ListCart));
        }
        public IActionResult deleteCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Book.Id == id)
                    {
                        dataCart.RemoveAt(i);
                    }
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                    return RedirectToAction(nameof(ListCart));
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ListCart()
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

                if (dataCart.Count > 0)
                {
                    ViewBag.carts = dataCart;
                    return View();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult updateCart(int id, int quantity)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                if (quantity > 0)
                {
                    for (int i = 0; i < dataCart.Count; i++)
                    {
                        if (dataCart[i].Book.Id == id)
                        {
                            dataCart[i].Quantity = quantity;
                        }
                        HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));

                    }
                }
                return Ok(quantity);
            }
            return BadRequest();
        }
        public Book getDetailBook(int id)
        {
            var book = db.Books.Find(id);
            return book;
        }
        public IActionResult Index()
        {
            var _book = getAllBook();
            ViewBag.book = _book;
            return View();
        }
        public List<Book> getAllBook()
        {
            return db.Books.ToList();
        }
    }
}
