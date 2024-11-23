using PerIpsumOriginal.Enums;
namespace PerIpsumOriginal.Models
{
    public static class ValoresNulosModel
    {
        public const string Nome = "Título";
        public const string Descricao = "Descrição da oportunidade";
        public const PaisEnum Pais = PaisEnum.Desconhecido;
        public const TipoEnum Tipo = TipoEnum.Desconhecido;
        public const string Link = "Linkar";
        public static DateOnly Data = new DateOnly(1, 1, 1);
        public const string Categorias = "Nenhuma categoria selecionada";
    }
}
