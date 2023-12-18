using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class SqlConnectionProvider
    {
        // use a factory method to ensure that there is only 
        // one method in the entire project that has the 
        // database connection string

        private static string connectionString = @"Data Source=E67\SQLEXPRESS;Initial Catalog=musedb;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
