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
    class DeleteUserProfile : IUserProfile<int>
    {
        public int Manage()
        {
            using (var cn = SingletonDB.Instance.GetDBConnection())
            {
                cn.Open();
                try
                {
                    string strCmd = "update UserProfile set UserProfileStatus=-1 where UserProfileId=@UserProfileId;" + "update UserAccess set UserAccessStatus=-1 where UserAccessUserProfileId=@UserProfileId;update LocalSystemBranch set LocalSystemBranchStatus=-1 where LocalSystemBranchUserProfileId=@UserProfileId;";
                    SqlCommand cmd = new SqlCommand(strCmd, cn);
                    cmd.Parameters.AddWithValue("@UserProfileId", UserProperties.selectedUserProfileID);
                    int deleteResult = cmd.ExecuteNonQuery();
                    return deleteResult;

                }
                catch (SqlException er)
                {
                    Console.WriteLine(er.ToString());
                    return 0;
                }
            }

        }
    }
}
