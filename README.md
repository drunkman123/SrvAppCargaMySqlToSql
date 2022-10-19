# SrvAppCargaMySqlToSql
## Descrição do Projeto
### O que é?
Trata-se de um serviço para fazer Select em banco de dados MySql e depois um Insert em outro banco Sql.
### Tecnologias utilizadas
C# .NET 6, SQL, MySql
### Desafios e Futuro
Este projeto nasceu da necessidade de transportar dados de um banco legado(ainda em uso) MySql para o novo em Sql.<br />
A maior dificuldade foi para acertar a parte das datas, visto que no MySql o banco não foi modelado corretamente possui diversas datas que ficam inválidas no Sql. A solução foi fazer no select do MySql o tratamento com CASE e assim evitar datas menores que 1753-01-01.<br />
```
var sql = @"SELECT 
			 	                    Codigo,
  				                    Impresso,
  				                    RE,
  				                    cpf,
  				                    CASE WHEN Assunto = "" THEN NULL end as Assunto,
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
```

Foi criado um método genérico GetLastCod para evitar repetição de código, no resto não deu pois sao diversos campos diferentes com tipos diferentes.

### Funcionamento
Basta chamar o método Start() na controller que entrará em loop eterno, mesmo com erros(que estão sendo salvos no servidor), o programa só irá parar caso a máquina onde esteja hospedada, seja desligada ou o serviço parado.<br />
-Primeiramente o programa consulta qual o último Codigo (primary key) no banco Sql.<br />
-Em posse desse código, faz um select com where apenas para os registros com codigo acima do último no mysql<br />
-pega esses dados, faz um tratamento (para trocar strings vazias) por nulos<br />
-faz um insert com esses dados no sql.<br />
-grava o horário de atualização e pausa antes de ir para a próxima folha, e fica assim no loop.<br />
Também foi feito um método para salvar a data da última atualização de cada Folha.<br />

