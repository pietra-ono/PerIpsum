namespace PerIpsumOriginal.Models
{
    public class AnotacaoModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Cor {  get; set; }
        public string UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
