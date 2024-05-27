namespace AMST4_Carousel.MVC.Models
{
    public class BaseInfo
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
