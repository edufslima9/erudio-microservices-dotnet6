using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model.Context;
using GeekShopping.IdentityServer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GeekShopping.IdentityServer.Initializer;
using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connection = builder.Configuration["PostgreSQLConnection:PostgreSQLConnectionString"];

builder.Services.AddDbContext<PostgreSQLContext>(options => options.UseNpgsql(connection));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<PostgreSQLContext>()
    .AddDefaultTokenProviders();

var buildIdentity = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
.AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
.AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
.AddInMemoryClients(IdentityConfiguration.Clients)
.AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

buildIdentity.AddDeveloperSigningCredential();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

using var scope = app.Services.CreateScope();
var dbService = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

dbService.Initialize();

app.MapRazorPages();

app.Run();
