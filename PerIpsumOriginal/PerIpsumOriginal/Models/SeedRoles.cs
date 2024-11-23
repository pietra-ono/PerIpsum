using Microsoft.AspNetCore.Identity;
using PerIpsumOriginal.Models;

public static class SeedRoles
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<UsuarioModel>>();

        string[] roleNames = { "Admin", "Parcerias", "Usuario" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Criar usuário Admin padrão
        var adminUser = new UsuarioModel
        {
            UserName = "admin@peripsum.com",
            Email = "admin@peripsum.com",
            EmailConfirmed = true,
            Nome = "Administrador"
        };

        if (await userManager.FindByEmailAsync(adminUser.Email) == null)
        {
            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
