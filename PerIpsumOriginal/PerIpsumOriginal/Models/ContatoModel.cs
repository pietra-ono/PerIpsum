namespace PerIpsumOriginal.Models
{
    public class ContatoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Problema { get; set; }
        public DateTime Data { get; set; } = DateTime.Today;
    }
}
