namespace PerIpsumOriginal.Models.SubModels
{
    public class ConteudoCategorias
    {
        public int ConteudoId { get; set; }
        public ConteudoModel Conteudo { get; set; }

        public int CategoriaId { get; set; }
        public CategoriaModel Categoria { get; set; }
    }
}
