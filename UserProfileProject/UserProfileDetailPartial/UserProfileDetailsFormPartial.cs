using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserProfileProject.Common;

namespace UserProfileProject
{
    public partial class UserProfileDetailsForm
    {
        #region Disable Elements
        void DisableElements()
        {
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
            foreach (Control control in pnlInput.Controls)
            {
                control.Enabled = false;
            }
            foreach (Control item in pnlSystemBranch.Controls)
            {
                item.Enabled = false;
            }
        }
        #endregion

        #region chechValidation
        private bool checkValidations()
        {
            string warningText = string.Empty;
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (String.IsNullOrWhiteSpace(txtDomain.Text) || String.IsNullOrWhiteSpace(txtFulName.Text))
            {
                warningText = "Username can not be empty!";
            }
            if (!emailRegex.IsMatch(txtEmail.Text))
            {
                warningText += "\nEmail Address," + txtEmail.Text + " doesnt matches the expected format.\nExcepted format: test@test.test";
            }
            if (!string.IsNullOrEmpty(warningText))
            {
                MessageBox.Show(warningText, "Attention");
                return false;
            }
            else
                return true;


        }
        #endregion

        #region DeleteOperations
        void DeleteUserProfile()
        {
            int issucessful = deleteUserProfile.Manage();
            if (issucessful != 0)
            {
                DisableElements();
                MessageBox.Show("User Profile Deleted.");
            }
            else
                MessageBox.Show("Problem happened while deleting the user profile...");
        }
        # endregion
    }
}
