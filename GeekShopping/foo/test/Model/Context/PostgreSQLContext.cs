using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer.Model.Context
{
    public class PostgreSQLContext : IdentityDbContext<ApplicationUser>
    {
        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options) { }
    }
}
