namespace AMST4_Carousel.MVC.Models
{
    /// <summary>
    /// <Author>Andrey Bertoletti</Author>
    /// </summary>
    public class Product : BaseInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Stock { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category? Category { get; set; } 

    }
}
