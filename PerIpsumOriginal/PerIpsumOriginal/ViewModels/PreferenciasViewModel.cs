using PerIpsumOriginal.Enums;

namespace PerIpsumOriginal.ViewModels
{
    public class PreferenciasViewModel
    {
        public List<TipoEnum> Tipos { get; set; }
        public List<PaisEnum> Paises { get; set; }
        public List<TipoEnum> SelectedTipos { get; set; }
        public List<PaisEnum> SelectedPaises { get; set; }
    }
}
