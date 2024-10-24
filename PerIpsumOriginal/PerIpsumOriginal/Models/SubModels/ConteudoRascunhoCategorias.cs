namespace PerIpsumOriginal.Models.SubModels
{
    public class ConteudoRascunhoCategorias
    {
        public int ConteudoRascunhoId { get; set; }
        public ConteudoRascunhoModel ConteudoRascunho { get; set; }

        public int CategoriaId { get; set; }
        public CategoriaModel Categoria { get; set; }
    }
}
