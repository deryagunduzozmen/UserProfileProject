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
    class GetActualUserAccessList:IUserProfile<DataTable>
    {
        public DataTable Manage()
        {
            DataTable dt=new DataTable();
            using (var cn = SingletonDB.Instance.GetDBConnection())
            {
                cn.Open();
                string strCmdUserAcces = "select up.UserProfileId,ulc.UserLevelCategoryId,up.UserProfileDomainName,up.UserProfileName,up.UserProfileMailAddress,up.UserProfileUserLevelToUserAdmin, up.UserProfileName,ls.LocalSystemName,ulc.UserLevelCategoryName from UserProfile up inner join UserAccess ua on ua.UserAccessUserProfileId=up.UserProfileId inner join LocalSystem ls on ls.LocalSystemId=ua.UserAccessLocalSystemId inner join UserLevelCategory ulc on ulc.UserLevelCategoryId=ua.UserAccessUserLevelCategoryId and ls.LocalSystemId=ulc.UserLevelCategoryLocalSystemId where up.UserProfileStatus=0 and ua.UserAccessStatus=0 and up.UserProfileId=" + UserProperties.selectedUserProfileID;

                try
                {
                    dt = CommonFunctions.GetDataTable(strCmdUserAcces, cn);
                }
                catch (SqlException er)
                {
                    Console.WriteLine(er.ToString());
                }
                return dt;
            }
        
        }
    }
}
