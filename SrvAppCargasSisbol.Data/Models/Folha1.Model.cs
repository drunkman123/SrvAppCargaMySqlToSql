using MySqlConnector;
using System.Data.SqlTypes;

namespace SrvAppCargasSisbol.Data.Models
{
    public class Folha1
    {
        private string? _historico;
        private string? _obs;
        private string? _cpf;
        private string? _NumOrd;
        private int? _codigo_geral;
        private int? _id_log_cadmanual;


        public int Codigo { get; set; }
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
        public int? id_assunto { get; set; }
        public string? Historico { get=> _historico; set
            {
                _historico = value?.Replace("'", "`");
            } }
        public string? CodigoBol { get; set; }
        public string? Lancador { get; set; }
        public DateTime? DataDig { get; set; }
        public TimeSpan? Hora { get; set; }
        public int? Assinado { get; set; }
        public int? ReAssinante { get; set; }
        public int? id_desc { get; set; }
        public string? obs
        {
            get => _obs; set
            {
                _obs = value?.Replace("'", " ");
            }
        }
        public int? cod_opm { get; set; }
        public int? id_ordenacao { get; set; }
        public int? parte_bol { get; set; }
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
