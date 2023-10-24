using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookStore.ViewModels
{
    public class UploadImageViewModel
    {
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }
    }
}
