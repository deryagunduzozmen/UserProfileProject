using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfileProject.Common
{
    public class CommonFunctions
    {
        public static DataTable GetDataTable(string str, SqlConnection cn)
        {
            // SqlCommand cmd = new SqlCommand(str, cn);
            //DataTable dt = new DataTable();
            DataSet dst = new DataSet();
            //disconnected architecture
            SqlDataAdapter adp = new SqlDataAdapter(str, cn);
            //connected archtecture
            // SqlDataReader myReader = cmd.ExecuteReader();
            adp.Fill(dst);
            //dt.Load(myReader);
            return dst.Tables[0];
        }

    }
}
