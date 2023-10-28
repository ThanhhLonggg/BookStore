using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
      
        public string Title { get; set; }
        
        public string Image { get; set; }

        public string Category { get; set; }
       
        [MaxLength(1000)]
        public string Description { get; set; }

        public ICollection<CartDetail> CartDetails { get; set; }

        public double Price { get; set; }
    }
}
