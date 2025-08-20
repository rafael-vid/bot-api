using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;

namespace core.Util
{
    using core.Util;
    using System.Collections.Generic;

    public class DbMerge
    {

        DataTable dt = new DataTable();
        AcessoDados acc = new AcessoDados();
        Settings app = new Settings();

        /* 
             ------- INCLUIR ESTES PARAMETROS NO  appsettings.json -------
            "BdMergeDev": "bsk_banco_dev", // Coloque somente o nome da base de desenvolvimento
            "BdMergeHomol": "bsk_banco_dev", // Coloque somente o nome da base de homologação
            "BdMergeProd": "bsk_banco", // Coloque somente o nome da base de produção
            "BdMergeMatriz": "bsk_banco_dev", // Coloque o nome da base que servira de estrutura matriz para a comparação de tabelas 
            "MySqlDbConnectionMatriz": "Server=192.168.0.1; Port=5301; Database=bsk_banco_dev; Uid=studioBrasuka; Pwd=Senha@Senha; Connect Timeout=30;Allow User Variables=True",
            "MySqlDbConnectionDev": "Server=192.168.0.1; Port=5301; Database=bsk_banco_dev; Uid=studioBrasuka; Pwd=Senha@Senha; Connect Timeout=30;Allow User Variables=True",
            "MySqlDbConnectionHomol": "Server=192.168.0.1; Port=5301; Database=bsk_banco_dev; Uid=studioBrasuka; Pwd=Senha@Senha; Connect Timeout=30;Allow User Variables=True",
            "MySqlDbConnectionProd": "Server=192.168.0.1; Port=5301; Database=bsk_banco; Uid=studioBrasuka; Pwd=Senha@Senha; Connect Timeout=30;Allow User Variables=True"
        */


        public string ExecuteDbCompare()
        {

            var bddev = LoadListaTabelaMysql(app.Appsettings("BdMergeDev"), app.Appsettings("MySqlDbConnectionDev"));
            var bdhomol = LoadListaTabelaMysql(app.Appsettings("BdMergehomol"), app.Appsettings("MySqlDbConnectionHomol"));
            var bdProd = LoadListaTabelaMysql( app.Appsettings("BdMergeProd"), app.Appsettings("MySqlDbConnectionProd"));

            Dictionary<string, int> tabLine = new Dictionary<string, int>();
            tabLine.Add("DEV", bddev.Rows.Count);
            tabLine.Add("HML", bdhomol.Rows.Count);
            tabLine.Add("PROD", bdProd.Rows.Count);

            var maxline = 0;
            var matriz = "";
            foreach (var item in tabLine)
            {
                if (maxline < item.Value)
                {
                    maxline = item.Value;
                    matriz = item.Key;
                }
            }

            var con = new DataTable();
            var strCon = "";
            if (matriz == "DEV")
            {
                con = bddev;
                strCon = app.Appsettings("MySqlDbConnectionDev");
            }
            if (matriz == "HML")
            {
                con = bdhomol;
                strCon = app.Appsettings("MySqlDbConnectionHomol");
            }
            if (matriz == "PROD")
            {
                con = bdProd;
                strCon = app.Appsettings("MySqlDbConnectionProd");
            }

            if(!String.IsNullOrEmpty(app.Appsettings("MySqlDbConnectionMatriz")))
            {
                con = LoadListaTabelaMysql(app.Appsettings("BdMergeMatriz"), app.Appsettings("MySqlDbConnectionMatriz"));
                strCon = app.Appsettings("MySqlDbConnectionMatriz");
                matriz = "CONFIG - " + app.Appsettings("BdMergeMatriz");
            }
            
            var code = @"<table width='800px'>
    <tr>
        <td  align='center' style='background:#000000; color:#FFFFFF;'>Banco de estrutura Matriz: "+ matriz + @"</td> 
    </tr>
    <tr>
        <td align='center'>Quantidade Tabelas: <strong style='color:#4700dd;'> DEV= " + bddev.Rows.Count + @"</strong>  <strong style='color:fd7543;'>HOMOL= " + bdhomol.Rows.Count + @"</strong>  <strong style='color:#11ff43;'>PROD= " + bdProd.Rows.Count + @"</strong> </td>
    </tr>
   <tr>
        <td align='center' style='background:#000000; color:#FFFFFF;'>" + strCon + @"</td>
    </tr>
     
</table>
<h1>Comparação de tabelas. </h1>
#COMPARE
<h1>Comparação de estrutura de colunas das tabelas. </h1>

";




            foreach (DataRow item in con.Rows)
            {
                code+= TableCompare(item[0].ToString());
            }

            //INCLUSÂO DE COMPARAÇÂO DE TABELAS
            code = code.Replace("#COMPARE", BaseCompare());

            return code;
        }



        public string BaseCompare()
        {

            var _codedev = "";
            var _codehomol = "";
            var _codeprod = "";

            var tabelaHtml = @"
 <table>

    <tr>

       #CODE
        </tr>
        </table>
<p>&nbsp</p> 
<p>&nbsp</p> 

";

            var tabelaHtmlHeader = @"<td>           
<table border='1' >
    <tr>
        <td colspan='4' align='center' style='background:#000000; color:#FFFFFF;'> #BASE</td>
       
    </tr>
    <tr>
        <td colspan='1' align='center'>&nbsp;</td>
 <td>&nbsp;DEV&nbsp;</td>
<td>&nbsp;HML&nbsp&nbsp;</td>
<td>&nbsp;PROD&nbsp;</td>
        
    </tr>
        #LINHATABELA
  
</table>
        </td>";

            var LinhaTabela = @"<tr>
        <td>#TABELA &nbsp;&nbsp;&nbsp;</td>
        <td #STATUSDEV width='25px' >&nbsp;</td>
        <td #STATUSHML  width='25px'>&nbsp;</td>
        <td #STATUSPROD  width='25px'>&nbsp;</td>
        </tr>";


            var tabledev = LoadListaTabelaMysql(app.Appsettings("BdMergeDev"), app.Appsettings("MySqlDbConnectionDev"));
            var tablehomol = LoadListaTabelaMysql(app.Appsettings("BdMergehomol"), app.Appsettings("MySqlDbConnectionHomol"));
            var tableProd = LoadListaTabelaMysql(app.Appsettings("BdMergeProd"), app.Appsettings("MySqlDbConnectionProd"));


            List<string> devDatble = new List<string>();
            foreach (DataRow item in tabledev.Rows)
            {
                devDatble.Add(item[0].ToString());
            }

            List<string> homolDatble = new List<string>();
            foreach (DataRow item in tablehomol.Rows)
            {
                homolDatble.Add(item[0].ToString());
            }

            List<string> prodDatble = new List<string>();
            foreach (DataRow item in tableProd.Rows)
            {
                prodDatble.Add(item[0].ToString());
            }

            List<EntidadeTabelaModelo> modeloDev = new List<EntidadeTabelaModelo>();
            foreach (var item in devDatble)
            {
                EntidadeTabelaModelo entidade = new EntidadeTabelaModelo();
                
                if (devDatble.Where(x => x.Equals(item)).FirstOrDefault() != null)
                    entidade.Dev = "TRUE";
                else
                    entidade.Dev = "FALSE";


                if (homolDatble.Where(x=>x.Equals(item)).FirstOrDefault()!=null)
                    entidade.Hml = "TRUE";
                else
                    entidade.Hml = "FALSE";

                if (prodDatble.Where(x => x.Equals(item)).FirstOrDefault() != null)
                    entidade.Prod = "TRUE";
                else
                    entidade.Prod = "FALSE";

                entidade.Nome = item;
                modeloDev.Add(entidade);
            }

            List<EntidadeTabelaModelo> modeloHomol = new List<EntidadeTabelaModelo>();
            foreach (var item in homolDatble)
            {
                EntidadeTabelaModelo entidade = new EntidadeTabelaModelo();

                if (devDatble.Where(x => x.Equals(item)).FirstOrDefault() != null)
                    entidade.Dev = "TRUE";
                else
                    entidade.Dev = "FALSE";


                if (homolDatble.Where(x => x.Equals(item)).FirstOrDefault() != null)
                    entidade.Hml = "TRUE";
                else
                    entidade.Hml = "FALSE";

                if (prodDatble.Where(x => x.Equals(item)).FirstOrDefault() != null)
                    entidade.Prod = "TRUE";
                else
                    entidade.Prod = "FALSE";

                entidade.Nome = item;
                modeloHomol.Add(entidade);
            }

            List<EntidadeTabelaModelo> modeloProd = new List<EntidadeTabelaModelo>();
            foreach (var item in prodDatble)
            {
                EntidadeTabelaModelo entidade = new EntidadeTabelaModelo();

                if (devDatble.Where(x => x.Equals(item)).FirstOrDefault() != null)
                    entidade.Dev = "TRUE";
                else
                    entidade.Dev = "FALSE";


                if (homolDatble.Where(x => x.Equals(item)).FirstOrDefault() != null)
                    entidade.Hml = "TRUE";
                else
                    entidade.Hml = "FALSE";

                if (prodDatble.Where(x => x.Equals(item)).FirstOrDefault() != null)
                    entidade.Prod = "TRUE";
                else
                    entidade.Prod = "FALSE";

                entidade.Nome = item;

                modeloProd.Add(entidade);
            }



            var tabelasDev = "";
            foreach (var item in modeloDev)
            {
                var dev = "";
                var homol = "";
                var prod = "";

                if (item.Dev == "TRUE")
                    dev = "style='background:green;'";
                else
                    dev = "style='background:red;'";

                if (item.Hml == "TRUE")
                    homol = "style='background:green;'";
                else
                    homol = "style='background:red;'";

                if (item.Prod == "TRUE")
                    prod = "style='background:green;'";
                else
                    prod = "style='background:red;'";

                tabelasDev += LinhaTabela.Replace("#TABELA", item.Nome).Replace("#STATUSDEV", dev).Replace("#STATUSHML", homol).Replace("#STATUSPROD", prod).Replace("#BASE", "DEV");
            }
            tabelasDev = tabelaHtmlHeader.Replace("#LINHATABELA", tabelasDev).Replace("#BASE", "DEV");

            var tabelasHml = "";
            foreach (var item in modeloHomol)
            {
                var dev = "";
                var homol = "";
                var prod = "";

                if (item.Dev == "TRUE")
                    dev = "style='background:green;'";
                else
                    dev = "style='background:red;'";

                if (item.Hml == "TRUE")
                    homol = "style='background:green;'";
                else
                    homol = "style='background:red;'";

                if (item.Prod == "TRUE")
                    prod = "style='background:green;'";
                else
                    prod = "style='background:red;'";

                tabelasHml += LinhaTabela.Replace("#TABELA", item.Nome).Replace("#STATUSDEV", dev).Replace("#STATUSHML", homol).Replace("#STATUSPROD", prod).Replace("#BASE", "HML"); ;

            }
            tabelasHml = tabelaHtmlHeader.Replace("#LINHATABELA", tabelasHml).Replace("#BASE", "HML");

            var tabelasProd = "";
            foreach (var item in modeloProd)
            {
                var dev = "";
                var homol = "";
                var prod = "";

                if (item.Dev == "TRUE")
                    dev = "style='background:green;'";
                else
                    dev = "style='background:red;'";

                if (item.Hml == "TRUE")
                    homol = "style='background:green;'";
                else
                    homol = "style='background:red;'";

                if (item.Prod == "TRUE")
                    prod = "style='background:green;'";
                else
                    prod = "style='background:red;'";

                tabelasProd += LinhaTabela.Replace("#TABELA", item.Nome).Replace("#STATUSDEV", dev).Replace("#STATUSHML", homol).Replace("#STATUSPROD", prod) ;

            }
            tabelasProd = tabelaHtmlHeader.Replace("#LINHATABELA", tabelasProd).Replace("#BASE", "PROD");   

            return tabelaHtml.Replace("#CODE", tabelasDev + tabelasHml + tabelasProd); 
        }


            public string TableCompare(string tabela)
        {
            var _codedev = "";
            var _codehomol = "";
            var _codeprod = "";

            var tabelaHtml = @"
 <table>

    <tr>

       #CODE
        </tr>
        </table>
<p>&nbsp</p> 
";

            var tabelaHtmlHeader = @"<td>           
<table border='1' >
    <tr>
        <td colspan='6' align='center' style='background:#000000; color:#FFFFFF;'>#NOMETABELA #BASE</td>
       
    </tr>
    <tr>
        <td colspan='3' align='center'>&nbsp;</td>
 <td>&nbsp;DEV&nbsp;</td>
<td>&nbsp;HML&nbsp&nbsp;;</td>
<td>&nbsp;PROD&nbsp;</td>
        
    </tr>
        #LINHATABELA
  <tr>
       <td colspan='6' align='center' style='background:#000000; color:#FFFFFF;''> #QTD Coluna(s)</td>
    </tr>
</table>
        </td>";

            var LinhaTabela = @"<tr #COR >
        <td>#NOMECOLUNA &nbsp;&nbsp;&nbsp;</td>
        <td>#TIPO &nbsp;</td>
        <td>#COLUNA &nbsp;</td>
        <td #STATUSDEV width='25px' >&nbsp;</td>
        <td #STATUSHML  width='25px'>&nbsp;</td>
        <td #STATUSPROD  width='25px'>&nbsp;</td>
        </tr>";


            var tabledev = LoadTabelaMysql(tabela, app.Appsettings("BdMergeDev"), app.Appsettings("MySqlDbConnectionDev"));
            var tablehomol = LoadTabelaMysql(tabela, app.Appsettings("BdMergehomol"), app.Appsettings("MySqlDbConnectionHomol"));
            var tableProd = LoadTabelaMysql(tabela, app.Appsettings("BdMergeProd"), app.Appsettings("MySqlDbConnectionProd"));

            Dictionary<string, int> tabLine = new Dictionary<string, int>();
            tabLine.Add("DEV", tabledev.Rows.Count);
            tabLine.Add("HML", tablehomol.Rows.Count);
            tabLine.Add("PROD", tableProd.Rows.Count);

            var maxcolun=0;
            foreach (var item in tabLine)
            {
                if (maxcolun < item.Value)
                    maxcolun = item.Value;
            }
            



            //############################### DEV #################################################

            EntidadeModeloList devDatble = new EntidadeModeloList();

            foreach (DataRow item in tabledev.Rows)
            {
                devDatble.Column.Add(new EntidadeModelo
                {
                    Atributo = item[0].ToString(),
                    Tipo = item[1].ToString(),
                    TipoColuna = item[2].ToString()
                });
            }

            if (tabledev.Rows.Count < maxcolun)
            {   
                for (int i = tabledev.Rows.Count; i < maxcolun; i++)
                {
                    devDatble.Column.Add(new EntidadeModelo
                    {
                        Atributo = "",
                        Tipo = "",
                        TipoColuna = ""
                    });
                }
            }

            //############################### FIM DEV #################################################



            //############################### HML #################################################
          
            EntidadeModeloList homolDatble = new EntidadeModeloList();
            foreach (DataRow item in tablehomol.Rows)
            {
                homolDatble.Column.Add(new EntidadeModelo
                {
                    Atributo = item[0].ToString(),
                    Tipo = item[1].ToString(),
                    TipoColuna = item[2].ToString()
                });
            }

            if (tablehomol.Rows.Count < maxcolun)
            {
                for (int i = tablehomol.Rows.Count; i < maxcolun; i++)
                {
                    homolDatble.Column.Add(new EntidadeModelo
                    {
                        Atributo = "",
                        Tipo = "",
                        TipoColuna = ""
                    });
                }
            }

            //############################### FIM HML #################################################



            //############################### PRODUCAO #################################################
            EntidadeModeloList ProdDatble = new EntidadeModeloList();
            foreach (DataRow item in tableProd.Rows)
            {
                ProdDatble.Column.Add(new EntidadeModelo
                {
                    Atributo = item[0].ToString(),
                    Tipo = item[1].ToString(),
                    TipoColuna = item[2].ToString()
                });
            }

            if (tableProd.Rows.Count < maxcolun)
            {
                for (int i = tableProd.Rows.Count; i < maxcolun; i++)
                {
                    ProdDatble.Column.Add(new EntidadeModelo
                    {
                        Atributo = "",
                        Tipo = "",
                        TipoColuna = ""
                    });
                }
            }
            //############################### FIM PRODUCAO #################################################



            //############################### DEV #################################################
            foreach (var item in devDatble.Column.OrderBy(x=>x.Atributo).ToList())
            {
                var cor = "";
                var cordev = "style='background:green;'";
                var corhml = "";
                var corprod = "";
                if (item.Atributo == "")
                {
                    cor = "style='background:red;'";
                    cordev = "style='background:red;'";
                }

               

                var _homolDatble = homolDatble.Column.Where(x => x.Atributo == item.Atributo).FirstOrDefault();
                if(_homolDatble != null)
                {
                    if(_homolDatble.TipoColuna == item.TipoColuna)
                    {
                        corhml = "style='background:green;";
                    }
                    else
                    {
                        corhml = "style='background:yellow;";
                    }
                }
                else
                {
                    corhml = "style='background:red;";
                }


                var _ProdDatble = ProdDatble.Column.Where(x => x.Atributo == item.Atributo).FirstOrDefault();
                if (_ProdDatble != null)
                {
                    if (_ProdDatble.TipoColuna == item.TipoColuna)
                    {
                        corprod = "style='background:green;";
                    }
                    else
                    {
                        corprod = "style='background:yellow;";
                    }
                }
                else
                {
                    corprod = "style='background:red;";
                }


                _codedev += LinhaTabela.Replace("#NOMECOLUNA", item.Atributo).Replace("#TIPO", item.Tipo).Replace("#COLUNA", item.TipoColuna).Replace("#COR", cor).Replace("#STATUSDEV", cordev).Replace("#STATUSHML", corhml).Replace("#STATUSPROD", corprod);
            }
            var codeDev = tabelaHtmlHeader.Replace("#LINHATABELA", _codedev).Replace("#NOMETABELA", tabela).Replace("#BASE", "Desenvolvimento").Replace("#QTD", tabledev.Rows.Count.ToString());
            
            //############################### HML #################################################
            foreach (var item in homolDatble.Column.OrderBy(x => x.Atributo).ToList())
            {
                var cor = "";
                var cordev = "";
                var corhml = "style='background:green;'";
                var corprod = "";

                if (item.Atributo == "")
                {
                    cor = "style='background:red;'";
                    corhml = "style='background:red;'";
                }

               

                var _devDatble = devDatble.Column.Where(x => x.Atributo == item.Atributo).FirstOrDefault();
                if (_devDatble != null)
                {
                    if (_devDatble.TipoColuna == item.TipoColuna)
                    {
                        cordev = "style='background:green;";
                    }
                    else
                    {
                        cordev = "style='background:yellow;";
                    }
                }
                else
                {
                    cordev = "style='background:red;";
                }


                var _ProdDatble = ProdDatble.Column.Where(x => x.Atributo == item.Atributo).FirstOrDefault();
                if (_ProdDatble != null)
                {
                    if (_ProdDatble.TipoColuna == item.TipoColuna)
                    {
                        corprod = "style='background:green;";
                    }
                    else
                    {
                        corprod = "style='background:yellow;";
                    }
                }
                else
                {
                    corprod = "style='background:red;";
                }


                _codehomol += LinhaTabela.Replace("#NOMECOLUNA", item.Atributo).Replace("#TIPO", item.Tipo).Replace("#COLUNA", item.TipoColuna).Replace("#COR", cor).Replace("#STATUSDEV", cordev).Replace("#STATUSHML", corhml).Replace("#STATUSPROD", corprod); 
            }
            var codehomol = tabelaHtmlHeader.Replace("#LINHATABELA", _codehomol).Replace("#NOMETABELA", tabela).Replace("#BASE", "Homologação").Replace("#QTD", tablehomol.Rows.Count.ToString());
           
            //############################### PROD #################################################
            foreach (var item in ProdDatble.Column.OrderBy(x => x.Atributo).ToList())
            {
                var cor = "";
                var cordev = "";
                var corhml = "";
                var corprod = "style='background:green;'";

                if (item.Atributo == "")
                {
                    cor = "style='background:red;'";
                    corprod = "style='background:red;'";
                }
                              

                var _devDatble = devDatble.Column.Where(x => x.Atributo == item.Atributo).FirstOrDefault();
                if (_devDatble != null)
                {
                    if (_devDatble.TipoColuna == item.TipoColuna)
                    {
                        cordev = "style='background:green;";
                    }
                    else
                    {
                        cordev = "style='background:yellow;";
                    }
                }
                else
                {
                    cordev = "style='background:red;";
                }


                var _homolDatble = homolDatble.Column.Where(x => x.Atributo == item.Atributo).FirstOrDefault();
                if (_homolDatble != null)
                {
                    if (_homolDatble.TipoColuna == item.TipoColuna)
                    {
                        corhml = "style='background:green;";
                    }
                    else
                    {
                        corhml = "style='background:yellow;";
                    }
                }
                else
                {
                    corhml = "style='background:red;";
                }

                _codeprod += LinhaTabela.Replace("#NOMECOLUNA", item.Atributo).Replace("#TIPO", item.Tipo).Replace("#COLUNA", item.TipoColuna).Replace("#COR", cor).Replace("#STATUSDEV", cordev).Replace("#STATUSHML", corhml).Replace("#STATUSPROD", corprod); ;
            }
            var codeProd = tabelaHtmlHeader.Replace("#LINHATABELA", _codeprod).Replace("#NOMETABELA", tabela).Replace("#BASE", "Produção").Replace("#QTD", tableProd.Rows.Count.ToString());




            return tabelaHtml.Replace("#CODE", codeDev + codehomol + codeProd);
        }

        public DataTable LoadTabelaMysql(string _tabela, string bd, string db)
        {
            return Get(@"SELECT COLUMN_NAME, DATA_TYPE, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_SCHEMA = '" + bd + "' AND TABLE_NAME = '" + _tabela + "';", db);
        }
        public DataTable LoadListaTabelaMysql(string bd, string db)
        {
            return Get(@"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES  WHERE  TABLE_SCHEMA = '" + bd + "';", db);
        }

        public DataTable Get(string qry, string db)
        {
            return acc.retornaDatatable(qry, db);
        }


        public class EntidadeModeloList
        {
            public List<EntidadeModelo> Column { get; set; } = new List<EntidadeModelo>();
        }

        public class EntidadeModelo
        {
            public string Atributo { get; set; }
            public string Tipo { get; set; }
            public string TipoColuna { get; set; }


        }

        public class EntidadeTabelaModelo
        {
            public string Nome { get; set; }
            public string Dev { get; set; }
            public string Hml { get; set; }
            public string Prod { get; set; }

        }


        public class AcessoDados
        {
            public int execute(string mSQL, string db)
            {
                int id = 0;

                using (MySqlConnection conexaoMySQL = AcessoDados.MySQLDao.getInstancia().getConexao(db))
                {

                    DataTable dt = new DataTable();
                    try
                    {
                        conexaoMySQL.Open();

                        MySqlCommand cmd = new MySqlCommand(mSQL.Replace("null", ""), conexaoMySQL);


                        cmd.ExecuteNonQuery();
                        id = Convert.ToInt32(cmd.LastInsertedId);

                    }
                    catch (MySqlException msqle)
                    {

                    }
                    finally
                    {
                        conexaoMySQL.Close();
                    }

                    return id;
                }
            }


            public DataTable retornaDatatable(string mSQL, string db)
            {

                using (MySqlConnection conexaoMySQL = AcessoDados.MySQLDao.getInstancia().getConexao(db))
                {

                    DataTable dt = new DataTable();
                    try
                    {
                        conexaoMySQL.Open();

                        MySqlCommand cmd = new MySqlCommand(mSQL.Replace("null", ""), conexaoMySQL);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                        da.Fill(dt);

                        return dt;
                    }
                    catch (MySqlException msqle)
                    {
                        return dt;
                    }
                    finally
                    {
                        conexaoMySQL.Close();
                    }


                }

            }


            public class MySQLDao
            {
                private static readonly MySQLDao instanciaMySQL = new MySQLDao();
                Settings app = new Settings();
                private MySQLDao() { }

                public static MySQLDao getInstancia()
                {
                    return instanciaMySQL;
                }

                public MySqlConnection getConexao(string db)
                {
                    string conn = db;
                    return new MySqlConnection(conn);
                }

            }
        }

    }
}
