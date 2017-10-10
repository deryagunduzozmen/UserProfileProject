using System;
using System.Collections.Generic;
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

        #region Add New User Profile
        private void AddNewUser()
        {
            //To add new userprofile, first add userprofile table take generated id and add records to useraccess and localsystembranch tables
            bool isallValidated = checkValidations();
            int updateResult = 0;
            if (isallValidated)
            {
                using (var cn = SingletonDB.Instance.GetDBConnection())
                {
                    cn.Open();
                    try
                    {
                        string strCmd = string.Format("insert into UserProfile (UserProfileStatus,                                 UserProfileAccount, UserProfileDomainName, UserProfileName, UserProfileMailAddress, UserProfileUserLevelToUserAdmin, UserProfileOperatorId,UserProfileTimeStamp) OUTPUT INSERTED.UserProfileId values (0,'{0}','{1}','{2}','{3}','{4}',1,getdate());", txtDomain.Text, txtDomain.Text, txtFulName.Text, txtEmail.Text, (chcIsAdmin.Checked ? "Y" : "N"));
                        SqlCommand cmd = new SqlCommand(strCmd, cn);
                        UserProperties.selectedUserProfileID = (int)cmd.ExecuteScalar();
                        if (UserProperties.selectedUserProfileID != 0)
                        {
                            updateResult = AddNewProfileDetails(cn);
                            if (updateResult != 0)
                            {
                                MessageBox.Show("User added sucessfully");
                                groupBoxFooter.Visible = true;
                                btnAddNewUser.Visible = false;
                                lblUserProfileId.Text =UserProperties.selectedUserProfileID.ToString();
                                if (!chcIsAdmin.Checked)
                                    DisableElements();
                            }
                            else
                                MessageBox.Show("Problem occured when adding new user profile");
                        }
                        else
                            MessageBox.Show("Problem occured when adding new user profile");
                    }
                    catch (SqlException er)
                    {
                        Console.WriteLine(er.ToString());
                    }

                }
            }
        }

        private int AddNewProfileDetails(SqlConnection cn)
        {
            string strCmdAddCheckBox = UpdateCheckBoxes(cn);
            string strCmdAddBraches = PrepareInserBranchesScript();
            string AddUserProfileDetails = strCmdAddBraches + strCmdAddCheckBox;
            SqlCommand sqlCmd = new SqlCommand(AddUserProfileDetails, cn);
            int result = sqlCmd.ExecuteNonQuery();
            return result;
        }

        private string PrepareInserBranchesScript()
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
                        strCmd += string.Format("insert into UserAccess (UserAccessStatus,UserAccessUserProfileId,UserAccessLocalSystemId,UserAccessUserLevelCategoryId)values(0,{0},(select LocalSystemId from LocalSystem where LocalSystemName='{1}'),{2});", UserProperties.selectedUserProfileID, permissionSystemNames[1] + " " + permissionSystemNames[2], control.SelectedValue);
                    }
                }
            }
            return strCmd;
        }

        #endregion
    }
}
