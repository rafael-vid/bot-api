
using core.Domain.Interfaces;


namespace core.Infra.Repository
{
    using MySql.Data.MySqlClient;
    using System.Data.SqlClient;

    public class RepositoryBase : IRepositoryBase
    {
        private readonly string _connectionString;
        private readonly string _myConnectionString;
        public RepositoryBase(string myConnectionString)
        {
            _myConnectionString = myConnectionString;
        }


        public MySqlConnection connMysql()
        {
            MySqlConnection connection = new MySqlConnection(_myConnectionString);

            return connection;
        }
    }
}
