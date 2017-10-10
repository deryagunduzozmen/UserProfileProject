using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfileProject.Common
{
    public sealed class SingletonDB
    {
        private static readonly SingletonDB instance = new SingletonDB();
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["userProfileConnectionString"].ConnectionString;
        //static SingletonDB()
        //{
        //}

        private SingletonDB()
        {
        }

        public static SingletonDB Instance
        {
            get
            {
                return instance;
            }
        }

        public SqlConnection GetDBConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
