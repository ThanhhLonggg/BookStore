using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Models;
using BookStore.Data;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        // Hiển thị giỏ hàng của người dùng
        public IActionResult Index(string userId)
        {
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Book)
                .FirstOrDefault(c => c.UserId == userId);

            return View(cart);
        }

        [Authorize]
        // Thêm sách vào giỏ hàng
        [HttpPost]
        public IActionResult AddToCart(int bookId, string userId, int quantity)
        {
            //var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            //if (book == null)
            //{
            //    return NotFound();
            //}

            //var cart = _context.Carts
            //    .Include(c => c.CartDetails)
            //    .ThenInclude(cd => cd.Book)
            //    .FirstOrDefault(c => c.UserId == userId);

            //if (cart == null)
            //{
            //    cart = new Cart { UserId = userId };
            //    _context.Carts.Add(cart);
            //}

            var cartDetail = _context.CartDetails.FirstOrDefault(cd => cd.BookId == bookId && cd.UserId == userId);
            if (cartDetail == null)
            {
                cartDetail = new CartDetail
                {
                    BookId = bookId,
                    UserId = userId,
                    Quantity = quantity,
                    
                };
                _context.CartDetails.Add(cartDetail);
            }
            else
            {
                cartDetail.Quantity += quantity;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", new { UserId = userId });
        }

        [Authorize]
        // Xóa sách khỏi giỏ hàng
        [HttpPost]
        public IActionResult RemoveFromCart(int cartDetailId)
        {
            var cartDetail = _context.CartDetails.FirstOrDefault(cd => cd.CartDetailId == cartDetailId);
            if (cartDetail != null)
            {
                _context.CartDetails.Remove(cartDetail);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { userId = cartDetail.UserId });
        }

        [Authorize]
        // Cập nhật số lượng sách trong giỏ hàng
        [HttpPost]
        public IActionResult UpdateQuantity(int cartDetailId, int quantity)
        {
            var cartDetail = _context.CartDetails.FirstOrDefault(cd => cd.CartDetailId == cartDetailId);
            if (cartDetail != null)
            {
                cartDetail.Quantity = quantity;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { userId = cartDetail.UserId });
        }
    }
}
