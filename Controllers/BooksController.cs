using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var book = await _context.Books
                    .FirstOrDefaultAsync(m => m.Id == id);

                var speakerViewModel = new BookModel()
                {
                    Id = book.Id,
                    Title = book.Title,
                    ExistingImage = book.Image,
                    Description = book.Description,
                    Category = book.Category,
                    Price = book.Price,
                };

                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize(Roles ="ADMIN")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Image,Description,Category,Price")] BookModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = ProcessUploadedFile(model);
                    Book book = new()
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Category = model.Category,
                        Price = model.Price,
                        Image = uniqueFileName
                    };

                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            var BookViewModel = new BookModel()
            {
                Id = book.Id,
                Title = book.Title,
                ExistingImage = book.Image,
                Description = book.Description,
                Category = book.Category,
                Price = book.Price,
            };

            if (book == null)
            {
                return NotFound();
            }
            return View(BookViewModel); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookModel model)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.Books.FindAsync(model.Id);
                book.Title = model.Title;
                book.Description = model.Description;
                book.Category = model.Category;
                book.Price = model.Price;

                if (model.Image != null)
                {
                    if (model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(_environment.WebRootPath, "images", model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    book.Image = ProcessUploadedFile(model);
                }
                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            var BookViewModel = new BookModel()
            {
                Id = book.Id,
                Title = book.Title,
                ExistingImage = book.Image,
                Description = book.Description,
                Category = book.Category,
                Price = book.Price,
            };
            if (book == null)
            {
                return NotFound();
            }

            return View(BookViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            //string deleteFileFromFolder = "wwwroot\\Uploads\\";
            string deleteFileFromFolder = Path.Combine(_environment.WebRootPath, "images");
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFileFromFolder, book.Image);
            _context.Books.Remove(book);
            if (System.IO.File.Exists(CurrentImage))
            {
                System.IO.File.Delete(CurrentImage);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeakerExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile(BookModel model)
        {
            string uniqueFileName = null;
            string path = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

    }
}
