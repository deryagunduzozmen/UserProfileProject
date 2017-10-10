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
    class GetActualUserBranchList:IUserProfile<DataTable>
    {
        public DataTable Manage()
        {
            DataTable dt = new DataTable();
            using (var cn = SingletonDB.Instance.GetDBConnection())
            {
                cn.Open();
                string strCmdUserBranch = "select up.UserProfileName,br.BranchName,br.BranchCode,ls.LocalSystemName,* from LocalSystemBranch lsb inner join UserProfile up on up.UserProfileId=lsb.LocalSystemBranchUserProfileId inner join LocalSystem ls on ls.LocalSystemId=lsb.LocalSystemBranchLocalSystemId inner join Branch br on br.BranchCode=lsb.LocalSystemBranchCode where up.UserProfileStatus=0 and lsb.LocalSystemBranchStatus=0 and up.UserProfileId=" + UserProperties.selectedUserProfileID;
                try
                {
                    dt = CommonFunctions.GetDataTable(strCmdUserBranch, cn);
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
