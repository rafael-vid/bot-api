
using core.Domain.Interfaces;


namespace core.Infra.Repository
{
    using core.Util;
    using MySql.Data.MySqlClient;
    using System.Data.SqlClient;

    public class RepositoryBaseDLL
    {

        public MySqlConnection connMysql()
        {
            Settings settings = new Settings();
            // String de conexão com o banco de dados
            string connectionString = settings.Appsettings("MySqlDbConnection");
            MySqlConnection connection = new MySqlConnection(connectionString);

            return connection;
        }
    }
}
