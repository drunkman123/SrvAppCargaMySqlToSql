using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvAppCargasSisbol.Data.Models
{
    public class Folha2
    {
        private string? _obs;
        private string? _SubUnidade;
        private string? _Destacamento;
        private string? _cpf;
        private string? _Assunto;
        private string? _Hora;
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
        public string? Unidade { get; set; }
        public string? SubUnidade
        {
            get => _SubUnidade; set
            {
                _SubUnidade = value?.Replace("'", " ");
            }
        }
        public int? dest_codopm { get; set; }
        public string? Destacamento
        {
            get => _Destacamento; set
            {
                _Destacamento = value?.Replace("'", " ");
            }
        }
        public DateTime? DataIncEEf { get; set; }
        public string? CodigoBol { get; set; }
        
        public string? Obs
        {
            get => _obs; set
            {
                _obs = value?.Replace("'", " ");
            }
        }
        public string? Lancador { get; set; }
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
        public string? conveniencia { get; set; }
        public int? id_desc { get; set; }
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
        public int? situacao { get; set; }
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
