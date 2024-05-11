namespace AMST4_Carousel.MVC.Models
{
    public class Product { 
       public Guid Id { get; set; }
    public string Brand { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public double Stock { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateBy { get; set; } = DateTime.Now;
    
    }
}
