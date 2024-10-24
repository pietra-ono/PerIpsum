using Microsoft.AspNetCore.Identity;

namespace PerIpsumOriginal.Models
{
    public class UsuarioModel : IdentityUser
    {
        public string Nome { get; set; }

    }
}
