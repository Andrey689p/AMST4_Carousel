using AMST4_Carousel.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AMST4.Carousel.MVC.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Size> Size { get; set; }
    }
}