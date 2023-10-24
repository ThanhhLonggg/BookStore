using System.ComponentModel.DataAnnotations;
using BookStore.Models;

namespace BookStore.ViewModels
{
    public class BookModel : EditImageViewModel
    {
        [Required]
        public int Id { get; set; }

        public string Title { get; set; }

        public IFormFile Image { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string Category { get; set; }

        public double Price { get; set; }
    }
}
