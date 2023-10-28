using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public string UserId { get; set; } 

        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
