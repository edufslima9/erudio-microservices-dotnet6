using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Models.Context
{
    public class PostgreSQLContext : DbContext
    {
        public PostgreSQLContext() { }
        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

    }
}
