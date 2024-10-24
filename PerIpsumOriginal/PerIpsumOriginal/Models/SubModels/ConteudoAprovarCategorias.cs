namespace PerIpsumOriginal.Models.SubModels
{
    public class ConteudoAprovarCategorias
    {
        public int ConteudoAprovarId { get; set; }
        public ConteudoAprovarModel ConteudoAprovar { get; set; }

        public int CategoriaId { get; set; }
        public CategoriaModel Categoria { get; set; }
    }
}
