using PerIpsumOriginal.Enums;
using PerIpsumOriginal.Models.SubModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerIpsumOriginal.Models
{
    public class ConteudoRascunhoModel
    {
        public int Id { get; set; }
        public TipoEnum Tipo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        [NotMapped]
        public IFormFile ImagemUpload { get; set; }
        public string Imagem { get; set; }
        public PaisEnum Pais { get; set; }
        public DateOnly Data { get; set; }

        public string UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; }

        public ICollection<ConteudoRascunhoCategorias> ConteudoRascunhoCategorias { get; set; }


    }
}
