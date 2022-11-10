using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly PostgreSQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(PostgreSQLContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;

            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Customer)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "eduardo-admin",
                Email = "eduardlimmadev@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (81) 91234-5678",
                FirstName = "Eduardo",
                LastName = "Admin"
            };

            _user.CreateAsync(admin, "Ed123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
            }).Result;

            ApplicationUser customer = new ApplicationUser()
            {
                UserName = "eduardo-customer",
                Email = "edufslima9@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (81) 91234-5678",
                FirstName = "Eduardo",
                LastName = "Customer"
            };

            _user.CreateAsync(customer, "Ed123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(customer, IdentityConfiguration.Customer).GetAwaiter().GetResult();

            var customerClaims = _user.AddClaimsAsync(customer, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{customer.FirstName} {customer.LastName}"),
                new Claim(JwtClaimTypes.GivenName, customer.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customer.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Customer),
            }).Result;
        }
    }
}
