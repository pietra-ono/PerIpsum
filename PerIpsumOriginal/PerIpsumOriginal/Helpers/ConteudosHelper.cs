using PerIpsumOriginal.Enums;

namespace PerIpsumOriginal.Helpers
{
    public static class ConteudosHelper
    {
        public static string GetColorForTipo(TipoEnum tipo)
        {
            return tipo switch
            {
                TipoEnum.Provas => "#009846",
                TipoEnum.Oportunidades => "#E2CB26",
                TipoEnum.Eventos => "#002279",
                _ => "gray"
            };
        }

        public static string GetTextForTipo(TipoEnum tipo)
        {
            return tipo switch
            {
                TipoEnum.Provas => "Prova",
                TipoEnum.Oportunidades => "Oport.",
                TipoEnum.Eventos => "Evento",
                _ => "desconhecido"
            };
        }

        public static string GetFlagForCountry(PaisEnum pais)
        {
            return pais switch
            {
                PaisEnum.Brasil => "/img/bandeiras/brasil.svg",
                PaisEnum.China => "/img/bandeiras/china.svg",
                PaisEnum.EstadosUnidos => "/img/bandeiras/eua.svg",
                PaisEnum.Alemanha => "/img/bandeiras/alemanha.svg",
                PaisEnum.Japao => "/img/bandeiras/japao.svg",
                PaisEnum.India => "/img/bandeiras/india.svg",
                PaisEnum.ReinoUnido => "/img/bandeiras/uk.svg",
                PaisEnum.Franca => "/img/bandeiras/franca.svg",
                PaisEnum.Italia => "/img/bandeiras/italia.svg",
                PaisEnum.Canada => "/img/bandeiras/canada.svg",
                PaisEnum.Russia => "/img/bandeiras/russia.svg",
                PaisEnum.Mexico => "/img/bandeiras/mexico.svg",
                PaisEnum.CoreiaDoSul => "/img/bandeiras/cosul.svg",
                PaisEnum.Australia => "/img/bandeiras/australia.svg",
                PaisEnum.Espanha => "/img/bandeiras/espanha.svg",
                PaisEnum.Indonesia => "/img/bandeiras/indonesia.svg",
                PaisEnum.Turquia => "/img/bandeiras/turquia.svg",
                PaisEnum.Holanda => "/img/bandeiras/holanda.svg",
                PaisEnum.ArabiaSaudita => "/img/bandeiras/arab.svg",
                PaisEnum.Suica => "/img/bandeiras/suica.svg",
                PaisEnum.Portugal => "/img/bandeiras/portugal.svg",
                PaisEnum.Irlanda => "/img/bandeiras/irlanda.svg",
                PaisEnum.IrlandaDoNorte => "/img/bandeiras/uk.svg",
                PaisEnum.Chile => "/img/bandeiras/chile.svg",
                PaisEnum.Argentina => "/img/bandeiras/argentina.svg",
                _ => "/img/bandeiras/alemanha.svg"
            };
        }
        public static string GetCountryDisplayName(PaisEnum pais)
        {
            return pais switch
            {
                PaisEnum.Brasil => "BRA",
                PaisEnum.EstadosUnidos => "EUA",
                PaisEnum.China => "CHI",
                PaisEnum.Alemanha => "ALE",
                PaisEnum.Japao => "JAP",
                PaisEnum.India => "IND",
                PaisEnum.ReinoUnido => "UK",
                PaisEnum.Franca => "FRA",
                PaisEnum.Italia => "ITA",
                PaisEnum.Canada => "CAN",
                PaisEnum.Russia => "RUS",
                PaisEnum.Mexico => "MEX",
                PaisEnum.CoreiaDoSul => "CORS",
                PaisEnum.Australia => "AUS",
                PaisEnum.Espanha => "ESP",
                PaisEnum.Indonesia => "INDO",
                PaisEnum.Turquia => "TURQ",
                PaisEnum.Holanda => "HOL",
                PaisEnum.ArabiaSaudita => "ARAB",
                PaisEnum.Suica => "SUI",
                PaisEnum.Portugal => "POR",
                PaisEnum.Irlanda => "IRL",
                PaisEnum.IrlandaDoNorte => "IRLN",
                PaisEnum.Chile => "CHIL",
                PaisEnum.Argentina => "ARG",
                _ => "Desconhecido"
            };
        }

        
    }
}
