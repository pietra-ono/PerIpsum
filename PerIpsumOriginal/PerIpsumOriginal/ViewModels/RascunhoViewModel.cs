using Microsoft.AspNetCore.Mvc.Rendering;
using PerIpsumOriginal.Models;

namespace PerIpsumOriginal.ViewModels
{
    public class RascunhoViewModel
    {
        public IEnumerable<ConteudoRascunhoModel> Rascunhos { get; set; }
        public ConteudoRascunhoModel Rascunho { get; set; }

        public IEnumerable<ConteudoModel> Conteudos { get; set; }
        public ConteudoModel ConteudoM { get; set; }

        public List<SelectListItem> Categorias { get; set; }

        public IEnumerable<FavoritoModel> Favoritos { get; set; }
        public List<int> ConteudosFavoritosIds { get; set; }
    }
}
