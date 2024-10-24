using PerIpsumOriginal.Models.SubModels;

namespace PerIpsumOriginal.Models
{
    public class CategoriaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<ConteudoRascunhoCategorias> ConteudoRascunhoCategorias { get; set;}
        public ICollection<ConteudoAprovarCategorias> ConteudoAprovarCategorias { get; set; }
        public ICollection<ConteudoCategorias> ConteudoCategorias { get; set; }


    }
}
