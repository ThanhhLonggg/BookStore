using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookStore.Models;

namespace BookStore.Models
{
    public class CartDetail
    {
        [Key]
        public int CartDetailId { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }
        
        public string UserId { get; set; }

        public int Quantity { get; set; }

        public double TotalPrice { get; set; }
    }
}
