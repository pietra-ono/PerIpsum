using Microsoft.AspNetCore.Mvc.Rendering;
using PerIpsumOriginal.Enums;
using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.ViewModels
{
    public class RascunhoViewModel
    {
        public IEnumerable<ConteudoRascunhoModel> Rascunhos { get; set; }
        public ConteudoRascunhoModel Rascunho { get; set; }

        public IEnumerable<ConteudoModel> Conteudos { get; set; }
        public ConteudoModel ConteudoM { get; set; }

        public IEnumerable<FavoritoModel> Favoritos { get; set; }
        public List<int> ConteudosFavoritosIds { get; set; }

        public IEnumerable<AnotacaoModel> Anotacoes { get; set; }
        public AnotacaoModel Anotacao { get; set; }

        public List<int> FavoritosIds { get; set; }

        public List<CalendarioModel> Eventos { get; set; }
        public List<TipoEnum> TiposSelecionados { get; set; } = new List<TipoEnum>();
        public List<PaisEnum> PaisesSelecionados { get; set; } = new List<PaisEnum>();
    }
}
