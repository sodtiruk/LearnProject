using LearnProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnProject.Data
{
    public class ECommerceDbContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;

        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {

        }

    }
}
