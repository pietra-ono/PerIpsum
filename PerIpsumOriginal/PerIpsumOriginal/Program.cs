using Microsoft.EntityFrameworkCore;
using PerIpsumOriginal.Data;
using Microsoft.AspNetCore.Identity;
using PerIpsumOriginal.Models;
using PerIpsumOriginal.Repositorios.IRepositorios;
using PerIpsumOriginal.Repositorios;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IContatoRepositorio, ContatoRepositorio>();
builder.Services.AddScoped<IConteudoAprovarRepositorio, ConteudoAprovarRepositorio>();
builder.Services.AddScoped<IConteudoRascunhoRepositorio, ConteudoRascunhoRepositorio>();
builder.Services.AddScoped<IConteudoRepositorio, ConteudoRepositorio>();
builder.Services.AddScoped<IAnotacaoRepositorio, AnotacaoRepositorio>();

builder.Services.AddDbContext<PerIpsumDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");
    options.UseSqlServer(connectionString);
});

builder.Services.AddDefaultIdentity<UsuarioModel>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<PerIpsumDbContext>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles.Initialize(services);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
