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
    class GetUserList : IUserProfile<DataTable>
    {
        public DataTable Manage()
        {
            DataTable dt = new DataTable();
            using (var cn = SingletonDB.Instance.GetDBConnection())
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("select us.UserProfileId as ProfileId,us.UserProfileDomainName+'-'+us.UserProfileName as 'Profile Name',us.UserProfileMailAddress as 'Profile Email', us.UserProfileUserLevelToUserAdmin as 'Is Admin' from UserProfile us where UserProfileStatus=0  order by UserProfileId", cn);
                    SqlDataReader myReader = cmd.ExecuteReader();
                   
                    dt.Load(myReader);
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
