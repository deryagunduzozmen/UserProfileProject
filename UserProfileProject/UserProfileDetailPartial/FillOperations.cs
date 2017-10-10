using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserProfileProject.Common;

namespace UserProfileProject
{
    public partial class UserProfileDetailsForm
    {
        //works when form opens, 
        #region Fill Entry Form
        private void FillDataFromDataBase()
        {
            lblUserProfileId.Text = UserProperties.selectedUserProfileID.ToString();
            DataTable dtUserAccess = getActualUserAccessList.Manage();
            DataTable dtUserBranch = getActualUserBranchList.Manage();
            if (dtUserAccess.Rows.Count > 0)
            {
                txtDomain.Text = dtUserAccess.Rows[0]["UserProfileDomainName"].ToString();
                txtFulName.Text = dtUserAccess.Rows[0]["UserProfileName"].ToString();
                txtEmail.Text = dtUserAccess.Rows[0]["UserProfileMailAddress"].ToString();
                chcIsAdmin.Checked = isAdmin = dtUserAccess.Rows[0]["UserProfileUserLevelToUserAdmin"].ToString() == "Y";
                SetPermissionComboboxes(dtUserAccess);
            }
            if (dtUserBranch.Rows.Count > 0)
                SetSystemBranchChechkboxes(dtUserBranch);
            if (!isAdmin)
                DisableElements();
        }
        private void FillPermissionComboboxes()
        {
            string[] permissionSystemNames = new string[3];
            foreach (ComboBox control in comboBoxesInForm)
            {
                if (control.Name.Contains("cmb_System_"))
                {
                    permissionSystemNames = control.Name.Split('_');
                    if (permissionSystemNames.Length > 2)
                        AssignDataSourcePermissionCmb(permissionSystemNames, control);
                }
            }
        }

        private void SetSystemBranchChechkboxes(DataTable dt)
        {
            string[] sytemBranchNames = new string[4];
            foreach (CheckBox control in chechBoxesInForm)
            {
                if (control.Name.Contains("chc_System_"))
                {
                    sytemBranchNames = control.Name.Split('_');
                    if (sytemBranchNames.Length > 3)
                    {
                        control.Checked = dt
    .AsEnumerable()
    .Where(row => row.Field<String>("LocalSystemName") == sytemBranchNames[1] + " " + sytemBranchNames[2] && row.Field<String>("BranchCode") == sytemBranchNames[3]).Any();

                    }
                }
            }
        }

        private void SetPermissionComboboxes(DataTable dt)
        {
            foreach (ComboBox control in comboBoxesInForm)
            {
                string[] permissionSystemNames = new string[3];
                if (control.Name.Contains("cmb_System_"))
                {
                    permissionSystemNames = control.Name.Split('_');
                    if (permissionSystemNames.Length > 2)
                    {
                        control.SelectedValue = dt
    .AsEnumerable()
    .Where(row => row.Field<String>("LocalSystemName") == permissionSystemNames[1] + " " + permissionSystemNames[2]).Select(r => r.Field<int>("UserLevelCategoryId")).FirstOrDefault();
                    }
                }
            }

        }
        #endregion

        #region Assign DataSource
        private void AssignDataSourcePermissionCmb(string[] permissionSystemNames, ComboBox cbmControl)
        {
            DataTable dt = getUserAccessList.Manage();
            DataTable tblFiltered = dt.AsEnumerable()
      .Where(row => row.Field<String>("LocalSystemName") == permissionSystemNames[1] + " " + permissionSystemNames[2])
      .CopyToDataTable();
            cbmControl.DataSource = tblFiltered;
            cbmControl.ValueMember = "UserLevelCategoryId";
            cbmControl.DisplayMember = "UserLevelCategoryName";
        }
        #endregion

    }
}
