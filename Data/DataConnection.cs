using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace StokTakip.Data
{
    public class DataConnection
    {
        private SqlConnection connection;
        private readonly IConfiguration _configuration;

        public DataConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            connection = new SqlConnection(connectionString);
        }

        public void Open()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        public void Close()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }
    }
}
