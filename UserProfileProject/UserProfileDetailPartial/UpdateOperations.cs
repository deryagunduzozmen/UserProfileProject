using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserProfileProject.Common;

namespace UserProfileProject
{
    public partial class UserProfileDetailsForm
    {
        #region  Update Operations
        private void UpdateEntredData()
        {
            using (var cn = SingletonDB.Instance.GetDBConnection())
            {
                cn.Open();
                try
                {
                    string strCmdUpdateCheckBox = UpdateCheckBoxes(cn);
                    string strCmdUpdateComboboxes = UpdateUserAcces();
                    string stsCmdUserProfileInfo = string.Format("update UserProfile set UserProfileDomainName='{0}',UserProfileName='{1}', UserProfileMailAddress='{2}',UserProfileUserLevelToUserAdmin='{3}' where UserProfileId={4};", txtDomain.Text, txtFulName.Text, txtEmail.Text, chcIsAdmin.Checked ? "Y" : "N", UserProperties.selectedUserProfileID);
                    string strCmd = stsCmdUserProfileInfo + strCmdUpdateComboboxes + strCmdUpdateCheckBox;
                    SqlCommand cmd = new SqlCommand(strCmd, cn);
                    int updateResult = (int)cmd.ExecuteNonQuery();
                    if (updateResult != 0)
                        MessageBox.Show("Updated Sucessfully");
                    else
                        MessageBox.Show("Problem occured when updating user profile");
                }
                catch (SqlException er)
                {
                    Console.WriteLine(er.ToString());
                    MessageBox.Show("Problem happend during update operation");
                }
            }
        }

        private string UpdateCheckBoxes(SqlConnection cn)
        {
            //to update chechkboxes, first records from datatable should be taken and compare to existing form chages
            string cmdUpdateCheckboxex = string.Empty;
            bool isActiveInDatabase;
            bool isPassiveInDatabase;
            bool isCheckBoxChecked;
            int localSystemBranchId;
            DataTable dt = new DataTable();
            DataTable filteredDt = new DataTable();
            string[] systemBranchNames = new string[4];
            string txtcmdBranchList = "select lsb.LocalSystemBranchId, lsb.LocalSystemBranchCode,ls.LocalSystemName,lsb.LocalSystemBranchStatus from LocalSystemBranch lsb inner join LocalSystem ls on ls.LocalSystemId=lsb.LocalSystemBranchLocalSystemId  where lsb.LocalSystemBranchUserProfileId=" + UserProperties.selectedUserProfileID;
            dt = CommonFunctions.GetDataTable(txtcmdBranchList, cn);

            foreach (CheckBox control in chechBoxesInForm)
            {
                if (control.Name.Contains("chc_System_"))
                {
                    systemBranchNames = control.Name.Split('_');

                    if (systemBranchNames.Length > 3)
                    {
                        var tempTable = dt.AsEnumerable().Where(row => row.Field<String>("LocalSystemName") == systemBranchNames[1] + " " + systemBranchNames[2] && row.Field<String>("LocalSystemBranchCode") == systemBranchNames[3]);
                        isCheckBoxChecked = control.Checked;
                        //if there isnt any record(active or passive), need to insert table
                        if (!tempTable.Any())
                        {
                            if (isCheckBoxChecked)
                                cmdUpdateCheckboxex += PrepareInsertScript(systemBranchNames);
                        }
                        else
                        {
                            filteredDt = tempTable.CopyToDataTable();
                            isActiveInDatabase = filteredDt.AsEnumerable().Where(row => row.Field<int>("LocalSystemBranchStatus") == 0).Any();
                            isPassiveInDatabase = filteredDt.AsEnumerable().Where(row => row.Field<int>("LocalSystemBranchStatus") == -1).Any();
                            localSystemBranchId = filteredDt.AsEnumerable().Select(r => r.Field<int>("LocalSystemBranchId")).FirstOrDefault();

                            if (isCheckBoxChecked && isPassiveInDatabase)  //activate
                                cmdUpdateCheckboxex += PrepareActivateDeactivateScript(localSystemBranchId, 0);
                            else if (!isCheckBoxChecked && isActiveInDatabase) //deactivate
                                cmdUpdateCheckboxex += PrepareActivateDeactivateScript(localSystemBranchId, -1);
                            else
                                cmdUpdateCheckboxex += "";
                        }
                    }
                }
            }

            return cmdUpdateCheckboxex;
        }

        private string PrepareActivateDeactivateScript(int localSystemBranchId, int activate)
        {
            string cmdActivate = string.Format("update LocalSystemBranch set LocalSystemBranchStatus={0} where LocalSystemBranchId={1};", activate, localSystemBranchId);
            return cmdActivate;
        }
        private string PrepareInsertScript(string[] systemBranchNames)
        {
            string cmdInsert = string.Format("insert into LocalSystemBranch (LocalSystemBranchStatus,LocalSystemBranchUserProfileId,LocalSystemBranchLocalSystemId,LocalSystemBranchCode) values(0,{0},(select LocalSystemId from LocalSystem where LocalSystemName='{1}'),'{2}');", UserProperties.selectedUserProfileID, systemBranchNames[1] + " " + systemBranchNames[2], systemBranchNames[3]);
            return cmdInsert;
        }

        private string UpdateUserAcces()
        {
            string strCmd = string.Empty;
            foreach (ComboBox control in comboBoxesInForm)
            {
                string[] permissionSystemNames = new string[3];
                if (control.Name.Contains("cmb_System_"))
                {
                    permissionSystemNames = control.Name.Split('_');
                    if (permissionSystemNames.Length > 2)
                    {
                        strCmd += string.Format("update UserAccess set UserAccessUserLevelCategoryId={0} where UserAccessLocalSystemId=(select LocalSystemId from LocalSystem where LocalSystemName='{1}');", control.SelectedValue != null ? control.SelectedValue : 0, permissionSystemNames[1] + " " + permissionSystemNames[2]);
                    }
                }
            }
            return strCmd;
        }
        #endregion

    }
}
