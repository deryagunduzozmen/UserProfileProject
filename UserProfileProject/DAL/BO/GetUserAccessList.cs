using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfileProject.Common;
using UserProfileProject.DAL.Interfaces;

namespace UserProfileProject.DAL.BO
{
    class GetUserAccessList : IUserProfile<DataTable>
    {
        public DataTable Manage()
        {
            DataTable dt = new DataTable();
            string cmd;
            using (var cn = SingletonDB.Instance.GetDBConnection())
            {
                cn.Open();
                try
                {
                    cmd = "select ls.LocalSystemName,ulc.UserLevelCategoryId,ulc.UserLevelCategoryName from userlevelcategory ulc inner join LocalSystem ls on ls.LocalSystemId=ulc.UserLevelCategoryLocalSystemId";
                    dt = CommonFunctions.GetDataTable(cmd, cn);
                }
                catch (SqlException er)
                {
                    Console.WriteLine(er.ToString());
                }
            }
            return dt;
        }

    }
}
