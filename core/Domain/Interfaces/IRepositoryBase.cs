namespace core.Domain.Interfaces
{
    using MySql.Data.MySqlClient;

    public interface IRepositoryBase
    {
        MySqlConnection connMysql();
    }
}