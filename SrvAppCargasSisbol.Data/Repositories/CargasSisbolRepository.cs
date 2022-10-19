using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using SrvAppCargasSisbol.Data.Models;
using System.Data.SqlClient;
using System.Text;

namespace SrvAppCargasSisbol.Data.Repositories
{
    public class CargasSisbolRepository
    {
        //conex bancos
        private string _ConnSisbolSql;
        private string _ConnSisbolMySql;

        //tabelas
        private string _Folha1MySql;
        private string _Folha2MySql;
        private string _Folha3MySql;
        private string _Folha4MySql;
        private string _Folha1Sql;
        private string _Folha2Sql;
        private string _Folha3Sql;
        private string _Folha4Sql;

        //variavel para ultima atualização
        private StringBuilder? UltimaAtu;


        private IHostEnvironment _hostEnvironment;
        readonly IConfiguration Configuration;

        public CargasSisbolRepository(IConfiguration Configuration, IHostEnvironment hostingEnvironment)
        {
            _ConnSisbolSql = Configuration.GetValue<string>("Conn_Sisbol_SQL");
            _ConnSisbolMySql = Configuration.GetValue<string>("Conn_Sisbol_MYSQL");

            _Folha1MySql = Configuration.GetValue<string>("Folha1MySql");
            _Folha2MySql = Configuration.GetValue<string>("Folha2MySql");
            _Folha3MySql = Configuration.GetValue<string>("Folha3MySql");
            _Folha4MySql = Configuration.GetValue<string>("Folha4MySql");

            _Folha1Sql = Configuration.GetValue<string>("Folha1Sql");
            _Folha2Sql = Configuration.GetValue<string>("Folha2Sql");
            _Folha3Sql = Configuration.GetValue<string>("Folha3Sql");
            _Folha4Sql = Configuration.GetValue<string>("Folha4Sql");

            UltimaAtu = new StringBuilder();
            _hostEnvironment = hostingEnvironment;
            this.Configuration = Configuration;
        }

        public void UltimaAtuali(DateTime data, string folha)
        {
            UltimaAtu.Append(data);
            UltimaAtu.Append(" - ");
            UltimaAtu.Append(folha);
            UltimaAtu.Append("\r");
        }

        public void GetErro(string exception)
        {
            string Folder = _hostEnvironment.ContentRootPath + @"\Erro\erro" + (DateTime.Now).ToString().Replace("/", "_").Replace(":", ".") + ".txt";
            string erro = exception + " " + DateTimeOffset.Now;
            if (!File.Exists(Folder))
            {
                File.WriteAllText(Folder, erro);
            }
        }

        private int GetLastCod(string tipoFolha) {
            SqlDataReader reader = null;
            int retorno = 0;

            var query = @"SELECT TOP 1 Codigo FROM " + tipoFolha + @" ORDER BY Codigo DESC";

            using (SqlConnection con = new SqlConnection(_ConnSisbolSql))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {
                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            retorno = (int)reader["Codigo"];
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }

            return retorno;
        }

        public void SaveUltimaAtu()
        {
            string Folder = _hostEnvironment.ContentRootPath + @"\Ultima Atualizacao\ultima atualização.txt";
            File.Delete(Folder);
            File.WriteAllText(Folder, UltimaAtu.ToString());
            UltimaAtu.Clear();
        }

        public void CargaFolha1() {
        //public object CargaFolha1() {

            var LastCod = GetLastCod("folha_1_");
            List<Folha1> md = new List<Folha1>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {

                var sql = @"SELECT 
			 	                    Codigo,
  				                    codigo_geral,
  				                    Impresso,
  				                    RE,
  				                    cpf,
  				                    id_assunto,
  				                    Historico,
  				                    CodigoBol,
  				                    Lancador,
                                    CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                    Hora,
  				                    Assinado,
  				                    ReAssinante,
  				                    id_desc,
  				                    obs,
  				                    cod_opm,
  				                    id_ordenacao,
  				                    parte_bol,
  				                    NumOrd,
  				                    tipo_nota,
  				                    CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
  				                    id_log_cadmanual
				                    FROM " + _Folha1MySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

                MySqlCommand com = new MySqlCommand(sql, con);
                com.Parameters.Clear();
                com.Parameters.Add("@ultimo_id", MySqlDbType.Int32);
                com.Parameters["@ultimo_id"].Value = LastCod;
                con.Open();

                try
                {
                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            var _md = new Folha1();
                            _md.codigo = (int)reader["Codigo"];
                            _md.codigoGeral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.CPF = (reader["cpf"] != DBNull.Value && reader["cpf"].ToString() != "") ? reader["cpf"].ToString().Trim() : null;
                            _md.idAssunto = reader["id_assunto"] == DBNull.Value ? null : (int)reader["id_assunto"];
                            _md.historico = reader["Historico"] == DBNull.Value ? null : reader["Historico"].ToString().Trim();
                            _md.codigoBol = reader["CodigoBol"] == DBNull.Value ? null : reader["CodigoBol"].ToString().Trim();
                            _md.lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.dataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.hora = reader["Hora"] == DBNull.Value ? null : (TimeSpan)reader["Hora"];
                            _md.assinado = reader["Assinado"] == DBNull.Value ? null : (int)reader["Assinado"];
                            _md.reAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.idDesc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.obs = reader["obs"] == DBNull.Value ? null : reader["obs"].ToString().Trim();
                            _md.codOpm = reader["cod_opm"] == DBNull.Value ? null : (int)reader["cod_opm"];
                            _md.idOrdenacao = reader["id_ordenacao"] == DBNull.Value ? null : (int)reader["id_ordenacao"];
                            _md.parteBol = reader["parte_bol"] == DBNull.Value ? null : (int)reader["parte_bol"];
                            _md.numOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.tipoNota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.dataLancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.idLogCadManual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;

            var query = @"INSERT INTO " + _Folha1Sql + @"
                                            (Codigo,
  				                            codigo_geral,
  				                            Impresso,
  				                            RE,
  				                            cpf,
  				                            id_assunto,
  				                            Historico,
  				                            CodigoBol,
  				                            Lancador,
  				                            DataDig,
  				                            Hora,
  				                            Assinado,
  				                            ReAssinante,
  				                            id_desc,
  				                            obs,
  				                            cod_opm,
  				                            id_ordenacao,
  				                            parte_bol,
  				                            NumOrd,
  				                            tipo_nota,
  				                            data_lancamento,
  				                            id_log_cadmanual)
                            VALUES (@Codigo,
	                              @codigo_geral, 	 
	                              @Impresso, 		 
	                              @RE, 				
	                              @cpf, 				
	                              @id_assunto, 		 
	                              @Historico, 		
	                              @CodigoBol, 		 
	                              @Lancador, 		 
	                              @DataDig, 			 
	                              @Hora, 			
	                              @Assinado, 		
	                              @ReAssinante, 		
	                              @id_desc, 			 
	                              @obs, 				 
	                              @cod_opm, 			
	                              @id_ordenacao, 	 
	                              @parte_bol,		 
	                              @NumOrd,			 
	                              @tipo_nota, 		
	                              @data_lancamento, 	 
	                              @id_log_cadmanual)";

            using (SqlConnection con = new SqlConnection(_ConnSisbolSql))
            {
                con.Open();

                SqlCommand command = con.CreateCommand();

                SqlTransaction transaction;
                transaction = con.BeginTransaction("Transaction");
                command.Connection = con;
                command.Transaction = transaction;
                try
                {
                    foreach (Folha1 dados in md)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Codigo", dados.codigo);
                        command.Parameters.AddWithValue("@codigo_geral", dados.codigoGeral == null ? DBNull.Value : dados.codigoGeral);
                        command.Parameters.AddWithValue("@Impresso", dados.impresso == null ? DBNull.Value : dados.impresso);
                        command.Parameters.AddWithValue("@RE", dados.RE == null ? DBNull.Value : dados.RE);
                        command.Parameters.AddWithValue("@cpf", (dados.CPF == null)? DBNull.Value: dados.CPF);
                        command.Parameters.AddWithValue("@id_assunto", dados.idAssunto == null ? DBNull.Value : dados.idAssunto);
                        command.Parameters.AddWithValue("@Historico", dados.historico == null ? DBNull.Value : dados.historico);
                        command.Parameters.AddWithValue("@CodigoBol", dados.codigoBol == null ? DBNull.Value : dados.codigoBol);
                        command.Parameters.AddWithValue("@Lancador", dados.lancador == null ? DBNull.Value : dados.lancador);
                        command.Parameters.AddWithValue("@DataDig", dados.dataDig == null ? DBNull.Value : dados.dataDig);
                        command.Parameters.AddWithValue("@Hora", dados.hora == null ? DBNull.Value : dados.hora);
                        command.Parameters.AddWithValue("@Assinado", dados.assinado == null ? DBNull.Value : dados.assinado);
                        command.Parameters.AddWithValue("@ReAssinante", dados.reAssinante == null ? DBNull.Value : dados.reAssinante);
                        command.Parameters.AddWithValue("@id_desc", dados.idDesc == null ? DBNull.Value : dados.idDesc);
                        command.Parameters.AddWithValue("@obs", dados.obs == null ? DBNull.Value : dados.obs);
                        command.Parameters.AddWithValue("@cod_opm", dados.codOpm == null ? DBNull.Value : dados.codOpm);
                        command.Parameters.AddWithValue("@id_ordenacao", dados.idOrdenacao == null ? DBNull.Value : dados.idOrdenacao);
                        command.Parameters.AddWithValue("@parte_bol", dados.parteBol == null ? DBNull.Value : dados.parteBol);
                        command.Parameters.AddWithValue("@NumOrd", dados.numOrd == null ? DBNull.Value : dados.numOrd);
                        command.Parameters.AddWithValue("@tipo_nota", dados.tipoNota == null ? DBNull.Value : dados.tipoNota);
                        command.Parameters.AddWithValue("@data_lancamento", dados.dataLancamento == null ? DBNull.Value :dados.dataLancamento);
                        command.Parameters.AddWithValue("@id_log_cadmanual", dados.idLogCadManual == null ? DBNull.Value : dados.idLogCadManual);
                        command.CommandText = query;
                        command.ExecuteNonQuery();


                    }
                    transaction.Commit();
                    UltimaAtuali(DateTime.Now, "folha 1");
                }

                catch (Exception e)
                {
                    transaction.Rollback();
                    con.Close();
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }

        }
        public void CargaFolha2()
        {

            //REPETIÇÃO DA CargaFolha1() apenas com mudanças nos campos

        }
        public void CargaFolha3()
        {
            //REPETIÇÃO DA CargaFolha1() apenas com mudanças nos campos

        }
        public void CargaFolha4()
        {
            //REPETIÇÃO DA CargaFolha1() apenas com mudanças nos campos

        }
    }
