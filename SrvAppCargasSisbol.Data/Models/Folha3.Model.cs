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
        private string? _cpf;
        private string? _NumOrd;
        private int? _codigo_geral;
        private int? _id_log_cadmanual;


        public int Codigo { get; set; }
        public int? Impresso { get; set; }
        public int? RE { get; set; }
        public string? cpf
        {
            get => _cpf;
            set
            {
                if (value == "")
                {
                    _cpf = null;
                }
                else { _cpf = value; }
            }
        }
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
        public int? codigo_geral
        {
            get => _codigo_geral;
            set
            {
                if (value == 0)
                {
                    _codigo_geral = null;
                }
                else { _codigo_geral = value; }
            }
        }
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
        public string? NumOrd
        {
            get => _NumOrd;
            set
            {
                if (value == "")
                {
                    _NumOrd = null;
                }
                else { _NumOrd = value?.Replace("'", " "); }
            }
        }
        public int? cod_opm { get; set; }
        public int? tipo_nota { get; set; }
        public DateTime? data_lancamento { get; set; }
        public int? id_log_cadmanual
        {
            get => _id_log_cadmanual;
            set
            {
                if (value == 0)
                {
                    _id_log_cadmanual = null;
                }
                else { _id_log_cadmanual = value; }
            }
        }
    }
}
