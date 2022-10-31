using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrvAppCargasSisbol.Data.Models
{
    public class Folha5Verso
    {
        private string? _obs;
        private string? _cpf;
        private string? _Motivo;
        private string? _Parecer;
        private string? _Assunto;
        private string? _Especie5;
        private string? _Junta;
        private string? _NumOrd;
        private string? _Hora;
        private int? _ano;
        private int? _codigo_geral;
        private int? _id_log_cadmanual;


        public int Codigo { get; set; }
        public int? Impresso { get; set; }
        public Single? RE { get; set; }
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
        public string? Especie5
        {
            get => _Especie5;
            set
            {
                if (value == "")
                {
                    _Especie5 = null;
                }
                else { _Especie5 = value?.Replace("'", " "); }
            }
        }

        public string? Junta
        {
            get => _Junta;
            set
            {
                if (value == "")
                {
                    _Junta = null;
                }
                else { _Junta = value?.Replace("'", " "); }
            }
        }
        public string? Motivo
        {
            get => _Motivo; 
            set
            {
                if(value == "")
                {
                    _Motivo = null;
                }
                else { _Motivo = value?.Replace("'", " "); }
            }
        }
        public int? DiasAfast { get; set; }
        public DateTime? DataIniAfast { get; set; }
        public string? Parecer
        {
            get => _Parecer;
            set
            {
                if (value == "")
                {
                    _Parecer = null;
                }
                else { _Parecer = value?.Replace("'", " "); }
            }
        }
        public string? CodigoBol { get; set; }
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
        public int? id_desc { get; set; }
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
        public int? ano
        {
            get => _ano;
            set
            {
                if (value == 0)
                {
                    _ano = null;
                }
                else { _ano = value; }
            }
        }
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
        public DateTime? DataTermAfast { get; set; }
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
