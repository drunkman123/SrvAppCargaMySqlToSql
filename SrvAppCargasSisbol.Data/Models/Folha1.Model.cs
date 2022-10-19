using MySqlConnector;
using System.Data.SqlTypes;

namespace SrvAppCargasSisbol.Data.Models
{
    public class Folha1
    {
        private string? _historico;
        private string? _obs;


        public int codigo { get; set; }
        public int? codigoGeral { get; set; }
        public int? impresso { get; set; }
        public int? RE { get; set; }
        public string? CPF { get; set; }
        public int? idAssunto { get; set; }
        public string? historico { get=> _historico; set
            {
                _historico = value?.Replace("'", "`");
            } }
        public string? codigoBol { get; set; }
        public string? lancador { get; set; }
        public DateTime? dataDig { get; set; }
        public TimeSpan? hora { get; set; }
        public int? assinado { get; set; }
        public int? reAssinante { get; set; }
        public int? idDesc { get; set; }
        public string? obs
        {
            get => _obs; set
            {
                _obs = value?.Replace("'", " ");
            }
        }
        public int? codOpm { get; set; }
        public int? idOrdenacao { get; set; }
        public int? parteBol { get; set; }
        public string? numOrd { get; set; }
        public int? tipoNota { get; set; }
        public DateTime? dataLancamento { get; set; }
        public int? idLogCadManual { get; set; }

    }
}
