using System.Collections;

namespace SrvAppCargasSisbol.Data.Models
{
    public class Folha10
    {
        private string? _obs;
        private string? _cpf;
        private string? _Assunto;
        private string? _NumOrd;
        private string? _Hora;
        private int? _codigo_geral;
        private string? _Historico;
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
        public string? Assunto
        {
            get => _Assunto;
            set
            {
                if (value == "")
                {
                    _Assunto = null;
                }
                else { _Assunto = value?.Replace("'", " "); }
            }
        }
        public string? CodigoBol { get; set; }
        public string? Historico
        {
            get => _Historico;
            set
            {
                if (value == "")
                {
                    _Historico = null;
                }
                else { _Historico = value?.Replace("'", "`"); }
            }
        }
        public string? Lancador { get; set; }
        public int? AjdSecRE { get; set; }
        public DateTime? DataDig { get; set; }
        public string? Hora
        {
            get => _Hora;
            set
            {
                if (value == "")
                {
                    _Hora = null;
                }
                else { _Hora = value; }
            }
        }
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
        public string? obs
        {
            get => _obs;
            set
            {
                if (value == "")
                {
                    _obs = null;
                }
                else { _obs = value?.Replace("'", " "); }
            }
        }
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
