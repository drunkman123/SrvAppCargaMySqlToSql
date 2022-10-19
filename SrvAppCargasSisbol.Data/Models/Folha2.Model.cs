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

        public int Codigo { get; set; }
        public int? Impresso { get; set; }
        public int? RE { get; set; }
        public string? cpf { get; set; }
        public string? Assunto { get; set; }
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
        public string? Hora { get; set; }
        public int? Assinado { get; set; }
        public int? ReAssinante { get; set; }
        public int? codigo_geral { get; set; }
        public string? conveniencia { get; set; }
        public int? id_desc { get; set; }
        public string? NumOrd { get; set; }
        public int? cod_opm { get; set; }
        public int? tipo_nota { get; set; }
        public int? situacao { get; set; }
        public DateTime? data_lancamento { get; set; }
        public int? id_log_cadmanual { get; set; }

    }
}
