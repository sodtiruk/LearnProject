using LearnProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
