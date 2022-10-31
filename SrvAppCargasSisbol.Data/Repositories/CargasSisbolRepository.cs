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
        private string _Folha5FrenteMySql;
        private string _Folha5VersoMySql;
        private string _Folha6MySql;
        private string _Folha8MySql;
        private string _Folha9MySql;
        private string _Folha10MySql;
        private string _Folha11FrenteMySql;

        private string _Folha1Sql;
        private string _Folha2Sql;
        private string _Folha3Sql;
        private string _Folha4Sql;
        private string _Folha5FrenteSql;
        private string _Folha5VersoSql;
        private string _Folha6Sql;
        private string _Folha8Sql;
        private string _Folha9Sql;
        private string _Folha10Sql;
        private string _Folha11FrenteSql;


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
            _Folha5FrenteMySql = Configuration.GetValue<string>("Folha5FrenteMySql");
            _Folha5VersoMySql = Configuration.GetValue<string>("Folha5VersoMySql");
            _Folha6MySql = Configuration.GetValue<string>("Folha6MySql");
            _Folha8MySql = Configuration.GetValue<string>("Folha8MySql");
            _Folha9MySql = Configuration.GetValue<string>("Folha9MySql");
            _Folha10MySql = Configuration.GetValue<string>("Folha10MySql");
            _Folha11FrenteMySql = Configuration.GetValue<string>("Folha11FrenteMySql");

            _Folha1Sql = Configuration.GetValue<string>("Folha1Sql");
            _Folha2Sql = Configuration.GetValue<string>("Folha2Sql");
            _Folha3Sql = Configuration.GetValue<string>("Folha3Sql");
            _Folha4Sql = Configuration.GetValue<string>("Folha4Sql");
            _Folha5FrenteSql = Configuration.GetValue<string>("Folha5FrenteSql");
            _Folha5VersoSql = Configuration.GetValue<string>("Folha5VersoSql");
            _Folha6Sql = Configuration.GetValue<string>("Folha6Sql");
            _Folha8Sql = Configuration.GetValue<string>("Folha8Sql");
            _Folha9Sql = Configuration.GetValue<string>("Folha9Sql");
            _Folha10Sql = Configuration.GetValue<string>("Folha10Sql");
            _Folha11FrenteSql = Configuration.GetValue<string>("Folha11FrenteSql");

            UltimaAtu = new StringBuilder();
            _hostEnvironment = hostingEnvironment;
            this.Configuration = Configuration;
        }

        public void UltimaAtuali(DateTime data, string folha)
        {
            UltimaAtu.Append(data);
            UltimaAtu.Append(" - ");
            UltimaAtu.Append(folha);
            UltimaAtu.Append(Environment.NewLine);
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
        public int GetLastCod(string tipoFolha)
        {
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
            var a = UltimaAtu.ToString();
            File.WriteAllText(Folder, UltimaAtu.ToString());
            UltimaAtu.Clear();
        }



        public void CargaFolha1()
        {

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
                            _md.Codigo = (int)reader["Codigo"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] != DBNull.Value && reader["cpf"].ToString() != "") ? reader["cpf"].ToString().Trim() : null;
                            _md.id_assunto = reader["id_assunto"] == DBNull.Value ? null : (int)reader["id_assunto"];
                            _md.Historico = reader["Historico"] == DBNull.Value ? null : reader["Historico"].ToString().Trim();
                            _md.CodigoBol = reader["CodigoBol"] == DBNull.Value ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : (TimeSpan)reader["Hora"];
                            _md.Assinado = reader["Assinado"] == DBNull.Value ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.obs = reader["obs"] == DBNull.Value ? null : reader["obs"].ToString().Trim();
                            _md.cod_opm = reader["cod_opm"] == DBNull.Value ? null : (int)reader["cod_opm"];
                            _md.id_ordenacao = reader["id_ordenacao"] == DBNull.Value ? null : (int)reader["id_ordenacao"];
                            _md.parte_bol = reader["parte_bol"] == DBNull.Value ? null : (int)reader["parte_bol"];
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
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
            if (md.Count > 0)
            {
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
                            //refatoração abaixo para deixar código mais clean e nao precisar fazer campo por campo
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            //
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 1");


        }
        public void CargaFolha2()
        {

            var LastCod = GetLastCod("folha_2_");
            List<Folha2> md = new List<Folha2>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
			 	                    Codigo,
  				                    Impresso,
  				                    RE,
  				                    cpf,
  				                    Assunto,
  				                    Unidade,
                                    SubUnidade,
                                    dest_codopm,
                                    Destacamento,
                                    CASE WHEN DataIncEEf < '1753-01-01' THEN NULL ELSE DataIncEEf end as DataIncEEf,
                                    CodigoBol,
                                    Obs,
                                    Lancador,
                                    CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    codigo_geral,
                                    conveniencia,
                                    id_desc,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    situacao,
  				                    CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                    id_log_cadmanual
				                    FROM " + _Folha2MySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha2();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value || reader["cpf"].ToString() == "") ? null : reader["cpf"].ToString().Trim();
                            _md.Assunto = reader["Assunto"] == DBNull.Value ? null : reader["Assunto"].ToString().Trim();
                            _md.Unidade = reader["Unidade"] == DBNull.Value ? null : reader["Unidade"].ToString().Trim();
                            _md.SubUnidade = reader["SubUnidade"] == DBNull.Value ? null : reader["SubUnidade"].ToString().Trim();
                            _md.dest_codopm = reader["dest_codopm"] == DBNull.Value ? null : (int)reader["dest_codopm"];
                            _md.Destacamento = reader["Destacamento"] == DBNull.Value ? null : reader["Destacamento"].ToString().Trim();
                            _md.DataIncEEf = reader["DataIncEEf"] == DBNull.Value ? null : (DateTime)reader["DataIncEEf"];
                            _md.CodigoBol = reader["CodigoBol"] == DBNull.Value ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Obs = reader["Obs"] == DBNull.Value ? null : reader["Obs"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.Assinado = reader["Assinado"] == DBNull.Value ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.conveniencia = reader["conveniencia"] == DBNull.Value ? null : reader["conveniencia"].ToString().Trim();
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = reader["cod_opm"] == DBNull.Value ? null : (int)reader["cod_opm"];
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.situacao = reader["situacao"] == DBNull.Value ? null : (int)reader["situacao"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha2Sql + @"
                                    (Codigo,
  				                    Impresso,
  				                    RE,
  				                    cpf,
  				                    Assunto,
  				                    Unidade,
                                    SubUnidade,
                                    dest_codopm,
                                    Destacamento,
                                    DataIncEEf,
                                    CodigoBol,
                                    Obs,
                                    Lancador,
                                    DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    codigo_geral,
                                    conveniencia,
                                    id_desc,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    situacao,
                                    data_lancamento,
                                    id_log_cadmanual)
                            VALUES (@Codigo,
  				                    @Impresso,
  				                    @RE,
  				                    @cpf,
  				                    @Assunto,
  				                    @Unidade,
                                    @SubUnidade,
                                    @dest_codopm,
                                    @Destacamento,
                                    @DataIncEEf,
                                    @CodigoBol,
                                    @Obs,
                                    @Lancador,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @codigo_geral,
                                    @conveniencia,
                                    @id_desc,
                                    @NumOrd,
                                    @cod_opm,
                                    @tipo_nota,
                                    @situacao,
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
                        foreach (Folha2 dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 2");

        }
        public void CargaFolha3()
        {
            //public object CargaFolha2() {
            var LastCod = GetLastCod("folha_3_");
            List<Folha3> md = new List<Folha3>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                    Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    DiasAfast,
                                    NumBolG,
                                    CASE WHEN DataBolG < '1753-01-01' THEN NULL ELSE DataBolG end as DataBolG,
                                    CASE WHEN DataIniAfast < '1753-01-01' THEN NULL ELSE DataIniAfast end as DataIniAfast,
                                    CodigoBol,
                                    Lancador,
                                    CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                    Assinado,
                                    ReAssinante,
                                    codigo_geral,
                                    id_desc,
                                    CASE WHEN DataTermAfast < '1753-01-01' THEN NULL ELSE DataTermAfast end as DataTermAfast,
                                    NumBolGerTerm,
                                    CASE WHEN dataBolGerTermAfast < '1753-01-01' THEN NULL ELSE dataBolGerTermAfast end as dataBolGerTermAfast,
                                    obs,
                                    ano,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                    id_log_cadmanual
				                    FROM " + _Folha3MySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha3();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value || reader["cpf"].ToString() == "") ? null : reader["cpf"].ToString().Trim();
                            _md.DiasAfast = reader["DiasAfast"] == DBNull.Value ? null : (int)reader["DiasAfast"];
                            _md.NumBolG = (reader["NumBolG"] == DBNull.Value || reader["NumBolG"].ToString() == "") ? null : reader["NumBolG"].ToString().Trim();
                            _md.DataBolG = reader["DataBolG"] == DBNull.Value ? null : (DateTime)reader["DataBolG"];
                            _md.DataIniAfast = reader["DataIniAfast"] == DBNull.Value ? null : (DateTime)reader["DataIniAfast"];
                            _md.CodigoBol = reader["CodigoBol"] == DBNull.Value ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Assinado = reader["Assinado"] == DBNull.Value ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.DataTermAfast = reader["DataTermAfast"] == DBNull.Value ? null : (DateTime)reader["DataTermAfast"];
                            _md.NumBolGerTerm = reader["NumBolGerTerm"] == DBNull.Value ? null : reader["NumBolGerTerm"].ToString().Trim();
                            _md.dataBolGerTermAfast = reader["dataBolGerTermAfast"] == DBNull.Value ? null : (DateTime)reader["dataBolGerTermAfast"];
                            _md.obs = reader["obs"] == DBNull.Value ? null : reader["obs"].ToString().Trim();
                            _md.ano = reader["ano"] == DBNull.Value ? null : (int)reader["ano"];
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = reader["cod_opm"] == DBNull.Value ? null : (int)reader["cod_opm"];
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha3Sql + @"
                                    (Codigo,
  				                    Impresso,
  				                    RE,
  				                    cpf,
  				                    DiasAfast,
  				                    NumBolG,
                                    DataBolG ,
                                    DataIniAfast ,
                                    CodigoBol,
                                    Lancador,
                                    DataDig,
                                    Assinado,
                                    ReAssinante,
                                    codigo_geral,
                                    id_desc,
                                    DataTermAfast,
                                    NumBolGerTerm,
                                    dataBolGerTermAfast,
                                    obs,
                                    ano,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    data_lancamento,
                                    id_log_cadmanual)
                            VALUES (@Codigo,
  				                    @Impresso,
  				                    @RE,
  				                    @cpf,
  				                    @DiasAfast,
                                    @NumBolG,
                                    @DataBolG ,
                                    @DataIniAfast ,
                                    @CodigoBol,
                                    @Lancador,
                                    @DataDig,
                                    @Assinado,
                                    @ReAssinante,
                                    @codigo_geral,
                                    @id_desc,
                                    @DataTermAfast,
                                    @NumBolGerTerm,
                                    @dataBolGerTermAfast,
                                    @obs,
                                    @ano,
                                    @NumOrd,
                                    @cod_opm,
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
                        foreach (Folha3 dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 3");

        }
        public void CargaFolha4()
        {
            var LastCod = GetLastCod("folha_4__");
            List<Folha4> md = new List<Folha4>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                    Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    AnoPAF,
                                    DiasAfast,
                                    CASE WHEN DataIniAfast < '1753-01-01' THEN NULL ELSE DataIniAfast end as DataIniAfast,
                                    BolTipo,
                                    CodigoBol,
                                    Lancador,
                                    CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    ano,
                                    CASE WHEN DataTermAfast < '1753-01-01' THEN NULL ELSE DataTermAfast end as DataTermAfast,
                                    obs,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                    id_log_cadmanual
				                    FROM " + _Folha4MySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha4();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value || reader["cpf"].ToString() == "") ? null : reader["cpf"].ToString().Trim();
                            _md.Assunto = (reader["Assunto"] == DBNull.Value || reader["Assunto"].ToString() == "") ? null : reader["Assunto"].ToString().Trim();
                            _md.AnoPAF = (reader["AnoPAF"] == DBNull.Value || (int)reader["AnoPAF"] == 0) ? null : (int)reader["AnoPAF"];
                            _md.DiasAfast = reader["DiasAfast"] == DBNull.Value ? null : reader["DiasAfast"].ToString().Trim();
                            _md.DataIniAfast = reader["DataIniAfast"] == DBNull.Value ? null : (DateTime)reader["DataIniAfast"];
                            _md.BolTipo = (reader["BolTipo"] == DBNull.Value || reader["BolTipo"].ToString() == "") ? null : reader["BolTipo"].ToString().Trim();
                            _md.CodigoBol = reader["CodigoBol"] == DBNull.Value ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = (reader["Hora"] == DBNull.Value || reader["Hora"].ToString() == "") ? null : reader["Hora"].ToString().Trim();
                            _md.Assinado = reader["Assinado"] == DBNull.Value ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.codigo_geral = (reader["codigo_geral"] == DBNull.Value || (int)reader["codigo_geral"] == 0) ? null : (int)reader["codigo_geral"];
                            _md.ano = (reader["ano"] == DBNull.Value || (int)reader["ano"] == 0) ? null : (int)reader["ano"];
                            _md.DataTermAfast = reader["DataTermAfast"] == DBNull.Value ? null : (DateTime)reader["DataTermAfast"];
                            _md.obs = reader["obs"] == DBNull.Value ? null : reader["obs"].ToString().Trim();
                            _md.NumOrd = (reader["NumOrd"] == DBNull.Value || reader["NumOrd"].ToString() == "") ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = reader["cod_opm"] == DBNull.Value ? null : (int)reader["cod_opm"];
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha4Sql + @"
                                    (Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    AnoPAF,
                                    DiasAfast,
                                    DataIniAfast,
                                    BolTipo,
                                    CodigoBol,
                                    Lancador,
                                    DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    ano,
                                    DataTermAfast,
                                    obs,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    data_lancamento,
                                    id_log_cadmanual)
                            VALUES (@Codigo,
                                    @Impresso,
                                    @RE,
                                    @cpf,
                                    @Assunto,
                                    @AnoPAF,
                                    @DiasAfast,
                                    @DataIniAfast,
                                    @BolTipo,
                                    @CodigoBol,
                                    @Lancador,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @id_desc,
                                    @codigo_geral,
                                    @ano,
                                    @DataTermAfast,
                                    @obs,
                                    @NumOrd,
                                    @cod_opm,
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
                        foreach (Folha4 dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 4");

        }
        public void CargaFolha5Frente()
        {
            var LastCod = GetLastCod("folha_5_");
            List<Folha5Frente> md = new List<Folha5Frente>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                    Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Motivo,
                                    DiasAfast,
                                    CASE WHEN DataIniAfast < '1753-01-01' THEN NULL ELSE DataIniAfast end as DataIniAfast,
                                    Parecer,
                                    CodigoBol,
                                    Lancador,
                                    CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    ano,
                                    obs,
                                    CASE WHEN DataTermAfast < '1753-01-01' THEN NULL ELSE DataTermAfast end as DataTermAfast,
                                    NumOrd,
                                    cod_opm,
                                    Assunto,
                                    Especie5,
                                    Junta,
                                    sit_med,
                                    sit_odont,
                                    tipo_nota,
                                    CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                    id_log_cadmanual                                     
				                    FROM " + _Folha5FrenteMySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha5Frente();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value) ? null : reader["cpf"].ToString().Trim();
                            _md.Motivo = (reader["Motivo"] == DBNull.Value) ? null : reader["Motivo"].ToString().Trim();
                            _md.DiasAfast = (reader["DiasAfast"] == DBNull.Value) ? null : (int)reader["DiasAfast"];
                            _md.DataIniAfast = reader["DataIniAfast"] == DBNull.Value ? null : (DateTime)reader["DataIniAfast"];
                            _md.Parecer = reader["Parecer"] == DBNull.Value ? null : reader["Parecer"].ToString().Trim();
                            _md.CodigoBol = (reader["CodigoBol"] == DBNull.Value) ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : reader["Hora"].ToString().Trim();
                            _md.Assinado = (reader["Assinado"] == DBNull.Value) ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.ano = (reader["ano"] == DBNull.Value) ? null : (int)reader["ano"];
                            _md.obs = (reader["obs"] == DBNull.Value) ? null : reader["obs"].ToString().Trim();
                            _md.DataTermAfast = reader["DataTermAfast"] == DBNull.Value ? null : (DateTime)reader["DataTermAfast"];
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = (reader["cod_opm"] == DBNull.Value) ? null : (int)reader["cod_opm"];
                            _md.Assunto = reader["Assunto"] == DBNull.Value ? null : reader["Assunto"].ToString().Trim();
                            _md.Especie5 = reader["Especie5"] == DBNull.Value ? null : reader["Especie5"].ToString().Trim();
                            _md.Junta = reader["Junta"] == DBNull.Value ? null : reader["Junta"].ToString().Trim();
                            _md.sit_med = reader["sit_med"] == DBNull.Value ? null : reader["sit_med"].ToString().Trim();
                            _md.sit_odont = reader["sit_odont"] == DBNull.Value ? null : reader["sit_odont"].ToString().Trim();
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha5FrenteSql + @"
                                    ( Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Motivo,
                                    DiasAfast,
                                    DataIniAfast,
                                    Parecer,
                                    CodigoBol,
                                    Lancador,
                                    DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    ano,
                                    obs,
                                    DataTermAfast,
                                    NumOrd,
                                    cod_opm,
                                    Assunto,
                                    Especie5,
                                    Junta,
                                    sit_med,
                                    sit_odont,
                                    tipo_nota,
                                    data_lancamento,
                                    id_log_cadmanual)
                            VALUES (@Codigo,
                                    @Impresso,
                                    @RE,
                                    @cpf,
                                    @Motivo,
                                    @DiasAfast,
                                    @DataIniAfast,
                                    @Parecer,
                                    @CodigoBol,
                                    @Lancador,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @id_desc,
                                    @codigo_geral,
                                    @ano,
                                    @obs,
                                    @DataTermAfast,
                                    @NumOrd,
                                    @cod_opm,
                                    @Assunto,
                                    @Especie5,
                                    @Junta,
                                    @sit_med,
                                    @sit_odont,
                                    @tipo_nota,
                                    @data_lancamento,
                                    @id_log_cadmanual
                                    )";

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
                        foreach (Folha5Frente dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 5 Frente");

        }
        public void CargaFolha5Verso()
        {
            var LastCod = GetLastCod("folha_5_2_");
            List<Folha5Verso> md = new List<Folha5Verso>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                    Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    Especie5,
                                    Junta,
                                    Motivo,
                                    DiasAfast,
                                    CASE WHEN DataIniAfast < '1753-01-01' THEN NULL ELSE DataIniAfast end as DataIniAfast,
                                    Parecer,
                                    CodigoBol,
                                    Lancador,
                                    CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    ano,
                                    obs,
                                    CASE WHEN DataTermAfast < '1753-01-01' THEN NULL ELSE DataTermAfast end as DataTermAfast,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                    id_log_cadmanual                                     
				                    FROM " + _Folha5VersoMySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha5Verso();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (Single)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value) ? null : reader["cpf"].ToString().Trim();
                            _md.Motivo = (reader["Motivo"] == DBNull.Value) ? null : reader["Motivo"].ToString().Trim();
                            _md.DiasAfast = (reader["DiasAfast"] == DBNull.Value) ? null : (int)reader["DiasAfast"];
                            _md.DataIniAfast = reader["DataIniAfast"] == DBNull.Value ? null : (DateTime)reader["DataIniAfast"];
                            _md.Parecer = reader["Parecer"] == DBNull.Value ? null : reader["Parecer"].ToString().Trim();
                            _md.CodigoBol = (reader["CodigoBol"] == DBNull.Value) ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : reader["Hora"].ToString().Trim();
                            _md.Assinado = (reader["Assinado"] == DBNull.Value) ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.ano = (reader["ano"] == DBNull.Value) ? null : (int)reader["ano"];
                            _md.obs = (reader["obs"] == DBNull.Value) ? null : reader["obs"].ToString().Trim();
                            _md.DataTermAfast = reader["DataTermAfast"] == DBNull.Value ? null : (DateTime)reader["DataTermAfast"];
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = (reader["cod_opm"] == DBNull.Value) ? null : (int)reader["cod_opm"];
                            _md.Assunto = reader["Assunto"] == DBNull.Value ? null : reader["Assunto"].ToString().Trim();
                            _md.Especie5 = reader["Especie5"] == DBNull.Value ? null : reader["Especie5"].ToString().Trim();
                            _md.Junta = reader["Junta"] == DBNull.Value ? null : reader["Junta"].ToString().Trim();
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha5VersoSql + @"
                                    ( Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Motivo,
                                    DiasAfast,
                                    DataIniAfast,
                                    Parecer,
                                    CodigoBol,
                                    Lancador,
                                    DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    ano,
                                    obs,
                                    DataTermAfast,
                                    NumOrd,
                                    cod_opm,
                                    Assunto,
                                    Especie5,
                                    Junta,
                                    tipo_nota,
                                    data_lancamento,
                                    id_log_cadmanual)
                            VALUES (@Codigo,
                                    @Impresso,
                                    @RE,
                                    @cpf,
                                    @Motivo,
                                    @DiasAfast,
                                    @DataIniAfast,
                                    @Parecer,
                                    @CodigoBol,
                                    @Lancador,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @id_desc,
                                    @codigo_geral,
                                    @ano,
                                    @obs,
                                    @DataTermAfast,
                                    @NumOrd,
                                    @cod_opm,
                                    @Assunto,
                                    @Especie5,
                                    @Junta,
                                    @tipo_nota,
                                    @data_lancamento,
                                    @id_log_cadmanual
                                    )";

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
                        foreach (Folha5Verso dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 5 Verso");

        }
        public void CargaFolha6()
        {
            var LastCod = GetLastCod("folha_6_");
            List<Folha6> md = new List<Folha6>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                    Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    Especie6,                                  
                                    DiasAfast,
                                    CASE WHEN DataIniAfast < '1753-01-01' THEN NULL ELSE DataIniAfast end as DataIniAfast,                                   
                                    CodigoBol,
                                    Lancador,
                                    CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    obs,
                                    CASE WHEN dataTermino < '1753-01-01' THEN NULL ELSE dataTermino end as dataTermino,
                                    bg,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                    id_lp_concessao,
                                    id_log_cadmanual                                     
				                    FROM " + _Folha6MySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha6();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value) ? null : reader["cpf"].ToString().Trim();
                            _md.Assunto = reader["Assunto"] == DBNull.Value ? null : reader["Assunto"].ToString().Trim();
                            _md.Especie6 = reader["Especie6"] == DBNull.Value ? null : reader["Especie6"].ToString().Trim();
                            _md.DiasAfast = (reader["DiasAfast"] == DBNull.Value) ? null : (int)reader["DiasAfast"];
                            _md.DataIniAfast = reader["DataIniAfast"] == DBNull.Value ? null : (DateTime)reader["DataIniAfast"];
                            _md.CodigoBol = (reader["CodigoBol"] == DBNull.Value) ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : reader["Hora"].ToString().Trim();
                            _md.Assinado = (reader["Assinado"] == DBNull.Value) ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.obs = (reader["obs"] == DBNull.Value) ? null : reader["obs"].ToString().Trim();
                            _md.dataTermino = reader["dataTermino"] == DBNull.Value ? null : (DateTime)reader["dataTermino"];
                            _md.bg = reader["bg"] == DBNull.Value ? null : reader["bg"].ToString().Trim();
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = (reader["cod_opm"] == DBNull.Value) ? null : (int)reader["cod_opm"];
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_lp_concessao = reader["id_lp_concessao"] == DBNull.Value ? null : (int)reader["id_lp_concessao"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha6Sql + @"
                                    ( Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    Especie6,                                  
                                    DiasAfast,
                                    DataIniAfast,                                   
                                    CodigoBol,
                                    Lancador,
                                    DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    obs,
                                    dataTermino,
                                    bg,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    data_lancamento,
                                    id_lp_concessao,
                                    id_log_cadmanual )
                            VALUES (@Codigo,
                                    @Impresso,
                                    @RE,
                                    @cpf,
                                    @Assunto,
                                    @Especie6,                                  
                                    @DiasAfast,
                                    @DataIniAfast,                                   
                                    @CodigoBol,
                                    @Lancador,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @id_desc,
                                    @codigo_geral,
                                    @obs,
                                    @dataTermino,
                                    @bg,
                                    @NumOrd,
                                    @cod_opm,
                                    @tipo_nota,
                                    @data_lancamento,
                                    @id_lp_concessao,
                                    @id_log_cadmanual
                                    )";

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
                        foreach (Folha6 dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 6");

        }
        public void CargaFolha8()
        {
            var LastCod = GetLastCod("folha_8_");
            List<Folha8> md = new List<Folha8>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                    Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    TipoFuncao,
                                    SitFuncao,
                                    CASE WHEN dataInicio < '1753-01-01' THEN NULL ELSE dataInicio end as dataInicio,
                                    CASE WHEN dataTermino < '1753-01-01' THEN NULL ELSE dataTermino end as dataTermino,
                                    CodigoBol,
                                    Lancador,
                                    CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    obs,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                    id_log_cadmanual                                                                         
				                    FROM " + _Folha8MySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha8();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value) ? null : reader["cpf"].ToString().Trim();
                            _md.Assunto = reader["Assunto"] == DBNull.Value ? null : reader["Assunto"].ToString().Trim();
                            _md.TipoFuncao = reader["TipoFuncao"] == DBNull.Value ? null : reader["TipoFuncao"].ToString().Trim();
                            _md.SitFuncao = reader["SitFuncao"] == DBNull.Value ? null : reader["SitFuncao"].ToString().Trim();
                            _md.dataInicio = reader["dataInicio"] == DBNull.Value ? null : (DateTime)reader["dataInicio"];
                            _md.dataTermino = reader["dataTermino"] == DBNull.Value ? null : (DateTime)reader["dataTermino"];
                            _md.CodigoBol = (reader["CodigoBol"] == DBNull.Value) ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : reader["Hora"].ToString().Trim();
                            _md.Assinado = (reader["Assinado"] == DBNull.Value) ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.obs = (reader["obs"] == DBNull.Value) ? null : reader["obs"].ToString().Trim();
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = (reader["cod_opm"] == DBNull.Value) ? null : (int)reader["cod_opm"];
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha8Sql + @"
                                    ( Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    TipoFuncao,
                                    SitFuncao,
                                    dataInicio,
                                    dataTermino,
                                    CodigoBol,
                                    Lancador,
                                    DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    obs,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    data_lancamento,
                                    id_log_cadmanual )
                            VALUES (@Codigo,
                                    @Impresso,
                                    @RE,
                                    @cpf,
                                    @Assunto,
                                    @TipoFuncao,
                                    @SitFuncao,
                                    @dataInicio,
                                    @dataTermino,
                                    @CodigoBol,
                                    @Lancador,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @id_desc,
                                    @codigo_geral,
                                    @obs,
                                    @NumOrd,
                                    @cod_opm,
                                    @tipo_nota,
                                    @data_lancamento,
                                    @id_log_cadmanual
                                    )";

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
                        foreach (Folha8 dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 8");

        }
        public void CargaFolha9()
        {
            var LastCod = GetLastCod("folha_9_");
            List<Folha9> md = new List<Folha9>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                Codigo,
                                Impresso,
                                RE,
                                cpf,
                                Assunto,
                                TipoPuni,
                                TempoPuni,
                                CodigoBol,
                                Historico,
                                Lancador,
                                AjdSecRE,
                                CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                Hora,
                                Assinado,
                                ReAssinante,
                                OPM,
                                Retificacao,
                                CASE WHEN DataPunicao < '1753-01-01' THEN NULL ELSE DataPunicao end as DataPunicao,
                                id_desc,
                                codigo_geral,
                                obs,
                                NumOrd,
                                cod_opm,
                                tipo_nota,
                                CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                id_log_cadmanual                                                                       
				                FROM " + _Folha9MySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha9();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value) ? null : reader["cpf"].ToString().Trim();
                            _md.Assunto = reader["Assunto"] == DBNull.Value ? null : reader["Assunto"].ToString().Trim();
                            _md.TipoPuni = reader["TipoPuni"] == DBNull.Value ? null : (int)reader["TipoPuni"];
                            _md.TempoPuni = reader["TempoPuni"] == DBNull.Value ? null : reader["TempoPuni"].ToString().Trim();
                            _md.CodigoBol = (reader["CodigoBol"] == DBNull.Value) ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Historico = reader["Historico"] == DBNull.Value ? null : reader["Historico"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.AjdSecRE = reader["AjdSecRE"] == DBNull.Value ? null : (int)reader["AjdSecRE"];
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : reader["Hora"].ToString().Trim();
                            _md.Assinado = (reader["Assinado"] == DBNull.Value) ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.OPM = reader["OPM"] == DBNull.Value ? null : reader["OPM"].ToString().Trim();
                            _md.Retificacao = reader["Retificacao"] == DBNull.Value ? null : reader["Retificacao"].ToString().Trim();
                            _md.DataPunicao = reader["DataPunicao"] == DBNull.Value ? null : (DateTime)reader["DataPunicao"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.obs = (reader["obs"] == DBNull.Value) ? null : reader["obs"].ToString().Trim();
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = (reader["cod_opm"] == DBNull.Value) ? null : (int)reader["cod_opm"];
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha9Sql + @"
                                    ( Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    TipoPuni,
                                    TempoPuni,
                                    CodigoBol,
                                    Historico,
                                    Lancador,
                                    AjdSecRE,
                                    DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    OPM,
                                    Retificacao,
                                    DataPunicao,
                                    id_desc,
                                    codigo_geral,
                                    obs,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    data_lancamento,
                                    id_log_cadmanual)
                            VALUES (@Codigo,
                                    @Impresso,
                                    @RE,
                                    @cpf,
                                    @Assunto,
                                    @TipoPuni,
                                    @TempoPuni,
                                    @CodigoBol,
                                    @Historico,
                                    @Lancador,
                                    @AjdSecRE,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @OPM,
                                    @Retificacao,
                                    @DataPunicao,
                                    @id_desc,
                                    @codigo_geral,
                                    @obs,
                                    @NumOrd,
                                    @cod_opm,
                                    @tipo_nota,
                                    @data_lancamento,
                                    @id_log_cadmanual
                                    )";

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
                        foreach (Folha9 dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 9");

        }
        public void CargaFolha10()
        {
            var LastCod = GetLastCod("folha_10_");
            List<Folha10> md = new List<Folha10>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                Codigo,
                                Impresso,
                                RE,
                                cpf,
                                Assunto,
                                CodigoBol,
                                Historico,
                                Lancador,
                                AjdSecRE,
                                CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                Hora,
                                Assinado,
                                ReAssinante,
                                codigo_geral,
                                id_desc,
                                obs,
                                NumOrd,
                                cod_opm,
                                tipo_nota,
                                CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                id_log_cadmanual                              
				                FROM " + _Folha10MySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha10();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value) ? null : reader["cpf"].ToString().Trim();
                            _md.Assunto = reader["Assunto"] == DBNull.Value ? null : reader["Assunto"].ToString().Trim();
                            _md.CodigoBol = (reader["CodigoBol"] == DBNull.Value) ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Historico = reader["Historico"] == DBNull.Value ? null : reader["Historico"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.AjdSecRE = reader["AjdSecRE"] == DBNull.Value ? null : (int)reader["AjdSecRE"];
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : reader["Hora"].ToString().Trim();
                            _md.Assinado = (reader["Assinado"] == DBNull.Value) ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.obs = (reader["obs"] == DBNull.Value) ? null : reader["obs"].ToString().Trim();
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = (reader["cod_opm"] == DBNull.Value) ? null : (int)reader["cod_opm"];
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha10Sql + @"
                                    ( Codigo,
                                    Impresso,
                                    RE,
                                    cpf,
                                    Assunto,
                                    CodigoBol,
                                    Historico,
                                    Lancador,
                                    AjdSecRE,
                                    DataDig,
                                    Hora,
                                    Assinado,
                                    ReAssinante,
                                    id_desc,
                                    codigo_geral,
                                    obs,
                                    NumOrd,
                                    cod_opm,
                                    tipo_nota,
                                    data_lancamento,
                                    id_log_cadmanual)
                            VALUES (@Codigo,
                                    @Impresso,
                                    @RE,
                                    @cpf,
                                    @Assunto,
                                    @CodigoBol,
                                    @Historico,
                                    @Lancador,
                                    @AjdSecRE,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @id_desc,
                                    @codigo_geral,
                                    @obs,
                                    @NumOrd,
                                    @cod_opm,
                                    @tipo_nota,
                                    @data_lancamento,
                                    @id_log_cadmanual
                                    )";

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
                        foreach (Folha10 dados in md)
                        {
                            command.Parameters.Clear();
                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados) == null ? DBNull.Value : prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();


                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 10");
        }
        public void CargaFolha11Frente()
        {
            var LastCod = GetLastCod("folha_11_");
            List<Folha11Frente> md = new List<Folha11Frente>();
            MySqlDataReader reader = null;
            using (MySqlConnection con = new MySqlConnection(_ConnSisbolMySql.ToString()))

            {
                var sql = @"SELECT 
                                Codigo,
                                Impresso,
                                RE,
                                cpf,
                                Assunto,
                                TipoCurEst,
                                CASE WHEN DataIniAfast < '1753-01-01' THEN NULL ELSE DataIniAfast end as DataIniAfast,
                                CASE WHEN DataTerAfast < '1753-01-01' THEN NULL ELSE DataTerAfast end as DataTerAfast,
                                NotaCurso,
                                ConceitoCurso,
                                DiploMed,
                                CodigoBol,
                                Lancador,
                                CASE WHEN DataDig < '1753-01-01' THEN NULL ELSE DataDig end as DataDig,
                                Hora,
                                Assinado,
                                ReAssinante,
                                id_desc,
                                codigo_geral,
                                obs,
                                NumOrd,
                                cod_opm,
                                id_curso,
                                tipo_nota,
                                CASE WHEN data_lancamento < '1753-01-01' THEN NULL ELSE data_lancamento end as data_lancamento,
                                id_log_cadmanual                      
				                FROM " + _Folha11FrenteMySql + @" WHERE Codigo > @ultimo_id LIMIT 1000";

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
                            var _md = new Folha11Frente();
                            _md.Codigo = (int)reader["Codigo"];
                            _md.Impresso = reader["Impresso"] == DBNull.Value ? null : (int)reader["Impresso"];
                            _md.RE = reader["RE"] == DBNull.Value ? null : (int)reader["RE"];
                            _md.cpf = (reader["cpf"] == DBNull.Value) ? null : reader["cpf"].ToString().Trim();
                            _md.Assunto = reader["Assunto"] == DBNull.Value ? null : reader["Assunto"].ToString().Trim();
                            _md.TipoCurEst = reader["TipoCurEst"] == DBNull.Value ? null : reader["TipoCurEst"].ToString().Trim();
                            _md.DataIniAfast = reader["DataIniAfast"] == DBNull.Value ? null : (DateTime)reader["DataIniAfast"];
                            _md.DataTerAfast = reader["DataTerAfast"] == DBNull.Value ? null : (DateTime)reader["DataTerAfast"];
                            _md.NotaCurso = reader["NotaCurso"] == DBNull.Value ? null : reader["NotaCurso"].ToString().Trim();
                            _md.ConceitoCurso = reader["ConceitoCurso"] == DBNull.Value ? null : reader["ConceitoCurso"].ToString().Trim();
                            _md.DiploMed = reader["DiploMed"] == DBNull.Value ? null : reader["DiploMed"].ToString().Trim();
                            _md.CodigoBol = (reader["CodigoBol"] == DBNull.Value) ? null : reader["CodigoBol"].ToString().Trim();
                            _md.Lancador = reader["Lancador"] == DBNull.Value ? null : reader["Lancador"].ToString().Trim();
                            _md.DataDig = reader["DataDig"] == DBNull.Value ? null : (DateTime)reader["DataDig"];
                            _md.Hora = reader["Hora"] == DBNull.Value ? null : reader["Hora"].ToString().Trim();
                            _md.Assinado = (reader["Assinado"] == DBNull.Value) ? null : (int)reader["Assinado"];
                            _md.ReAssinante = reader["ReAssinante"] == DBNull.Value ? null : (int)reader["ReAssinante"];
                            _md.id_desc = reader["id_desc"] == DBNull.Value ? null : (int)reader["id_desc"];
                            _md.codigo_geral = reader["codigo_geral"] == DBNull.Value ? null : (int)reader["codigo_geral"];
                            _md.obs = (reader["obs"] == DBNull.Value) ? null : reader["obs"].ToString().Trim();
                            _md.NumOrd = reader["NumOrd"] == DBNull.Value ? null : reader["NumOrd"].ToString().Trim();
                            _md.cod_opm = (reader["cod_opm"] == DBNull.Value) ? null : (int)reader["cod_opm"];
                            _md.tipo_nota = reader["tipo_nota"] == DBNull.Value ? null : (int)reader["tipo_nota"];
                            _md.id_curso = reader["id_curso"] == DBNull.Value ? null : (int)reader["id_curso"];
                            _md.data_lancamento = reader["data_lancamento"] == DBNull.Value ? null : (DateTime)reader["data_lancamento"];
                            _md.id_log_cadmanual = reader["id_log_cadmanual"] == DBNull.Value ? null : (int)reader["id_log_cadmanual"];
                            md.Add(_md);
                        }
                    }
                }
                catch (Exception e)
                {
                    con.Close();
                    throw;
                }

                finally
                {
                    con.Close();
                }

            }
            //return md;
            if (md.Count > 0)
            {
                var query = @"INSERT INTO " + _Folha11FrenteSql + @"
                                    (  Codigo,
                                        Impresso,
                                        RE,
                                        cpf,
                                        Assunto,
                                        TipoCurEst,
                                        DataIniAfast,
                                        DataTerAfast,
                                        NotaCurso,
                                        ConceitoCurso,
                                        DiploMed,
                                        CodigoBol,
                                        Lancador,
                                        DataDig,
                                        Hora,
                                        Assinado,
                                        ReAssinante,
                                        id_desc,
                                        codigo_geral,
                                        obs,
                                        NumOrd,
                                        cod_opm,
                                        id_curso,
                                        tipo_nota,
                                        data_lancamento,
                                        id_log_cadmanual)
                            VALUES (@Codigo,
                                    @Impresso,
                                    @RE,
                                    @cpf,
                                    @Assunto,
                                    @TipoCurEst,
                                    @DataIniAfast,
                                    @DataTerAfast,
                                    @NotaCurso,
                                    @ConceitoCurso,
                                    @DiploMed,
                                    @CodigoBol,
                                    @Lancador,
                                    @DataDig,
                                    @Hora,
                                    @Assinado,
                                    @ReAssinante,
                                    @id_desc,
                                    @codigo_geral,
                                    @obs,
                                    @NumOrd,
                                    @cod_opm,
                                    @id_curso,
                                    @tipo_nota,
                                    @data_lancamento,
                                    @id_log_cadmanual
                                    )";

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
                        foreach (Folha11Frente dados in md)
                        {
                            command.Parameters.Clear();

                            foreach (var prop in dados.GetType().GetProperties())
                            {
                                command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(dados)== null? DBNull.Value: prop.GetValue(dados));
                            }
                            command.CommandText = query;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
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
            UltimaAtuali(DateTime.Now, "folha 11 Frente");
        }

    }
}
