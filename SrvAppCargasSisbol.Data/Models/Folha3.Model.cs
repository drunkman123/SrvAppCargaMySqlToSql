using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvAppCargasSisbol.Data.Models
{
    public class Folha3
    {
        private string? _obs;
        private string? _NumBolG;


        public int Codigo { get; set; }
        public int? Impresso { get; set; }
        public int? RE { get; set; }
        public string? cpf { get; set; }
        public int? DiasAfast { get; set; }
        public string? NumBolG
        {
            get => _NumBolG; set
            {
                _NumBolG = value?.Replace("'", " ");
            }
        }
        public DateTime? DataBolG { get; set; }
        public DateTime? DataIniAfast { get; set; }
        public string? CodigoBol { get; set; }
        public string? Lancador { get; set; }
        public DateTime? DataDig { get; set; }
        public int? Assinado { get; set; }
        public int? ReAssinante { get; set; }
        public int? codigo_geral { get; set; }
        public int? id_desc { get; set; }
        public DateTime? DataTermAfast { get; set; }
        public string? NumBolGerTerm { get; set; }
        public DateTime? dataBolGerTermAfast { get; set; }
        public string? obs
        {
            get => _obs; set
            {
                _obs = value?.Replace("'", " ");
            }
        }

        public int? ano { get; set; }
        public string? NumOrd { get; set; }
        public int? cod_opm { get; set; }
        public int? tipo_nota { get; set; }
        public DateTime? data_lancamento { get; set; }
        public int? id_log_cadmanual { get; set; }        

    }
}
