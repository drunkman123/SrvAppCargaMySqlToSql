using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvAppCargasSisbol.Data.Models
{
    public class Folha4
    {
        private string? _obs;


        public int Codigo { get; set; }
        public int? Impresso { get; set; }
        public int? RE { get; set; }
        public string? cpf { get; set; }
        public string? Assunto { get; set; }
        public int? AnoPAF { get; set; }
        public string? DiasAfast { get; set; }
        public DateTime? DataIniAfast { get; set; }
        public string? BolTipo { get; set; }
        public string? CodigoBol { get; set; }
        public string? Lancador { get; set; }
        public DateTime? DataDig { get; set; }
        public string? Hora { get; set; }
        public int? Assinado { get; set; }
        public int? ReAssinante { get; set; }
        public int? id_desc { get; set; }
        public int? codigo_geral { get; set; }
        public int? ano { get; set; }
        public DateTime? DataTermAfast { get; set; }
        public string? obs
        {
            get => _obs; set
            {
                _obs = value?.Replace("'", " ");
            }
        }
        public string? NumOrd { get; set; }
        public int? cod_opm { get; set; }
        public int? tipo_nota { get; set; }
        public DateTime? data_lancamento { get; set; }
        public int? id_log_cadmanual { get; set; }

    }
}
