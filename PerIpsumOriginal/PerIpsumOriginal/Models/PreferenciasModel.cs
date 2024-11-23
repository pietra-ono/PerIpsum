using PerIpsumOriginal.Enums;

namespace PerIpsumOriginal.Models
{
    public class PreferenciasModel
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public TipoEnum Tipo { get; set; }
        public PaisEnum Pais { get; set; }

        public virtual UsuarioModel Usuario { get; set; }
    }
}
