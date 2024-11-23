using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerIpsumOriginal.Models
{
    public class CalendarioModel
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } // Chave para IdentityUser
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateOnly Data { get; set; } // Inclui horário para flexibilidade

        public virtual UsuarioModel Usuario { get; set; }
    }
}
