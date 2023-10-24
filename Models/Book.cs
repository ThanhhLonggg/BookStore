using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
