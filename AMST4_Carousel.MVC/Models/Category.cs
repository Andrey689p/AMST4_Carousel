using Microsoft.VisualBasic;

namespace AMST4_Carousel.MVC.Models
{
    /// <summary>
    /// <Author>Andrey Bertoletti</Author>
    /// </summary>
    public class Category : BaseInfo
    {
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public virtual List<Product>? Products { get; set; }
    }
}
