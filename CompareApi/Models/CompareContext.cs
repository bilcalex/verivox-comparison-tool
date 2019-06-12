using Microsoft.EntityFrameworkCore;

namespace CompareApi.Models
{
    public class CompareContext : DbContext
    {
        public CompareContext(DbContextOptions<CompareContext> options)
            : base(options)
        {
        }

        public DbSet<ProductItem> ProductItems { get; set; }
    }
}