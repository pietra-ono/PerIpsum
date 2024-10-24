namespace PerIpsumOriginal.Models
{
    public class FavoritoModel
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
        public int ConteudoId { get; set; }
        public virtual ConteudoModel Conteudo { get; set; }
    }
}
