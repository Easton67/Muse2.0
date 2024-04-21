using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SqlConnectionProvider
    {
        private static string connectionString = @"Data Source=E67\SQLEXPRESS;Initial Catalog=musedb;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
