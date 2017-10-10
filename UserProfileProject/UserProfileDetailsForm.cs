using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserProfileProject.Common;
using UserProfileProject.DAL.BO;
using UserProfileProject.DAL.Interfaces;

namespace UserProfileProject
{
    public partial class UserProfileDetailsForm : Form
    {
        public bool isAdmin;
        private readonly IUserProfile<DataTable> getUserAccessList;
        private readonly IUserProfile<DataTable> getActualUserAccessList;
        private readonly IUserProfile<DataTable> getActualUserBranchList;
        private readonly IUserProfile<int> deleteUserProfile;
        IEnumerable<ComboBox> comboBoxesInForm;
        IEnumerable<CheckBox> chechBoxesInForm;
        public UserProfileDetailsForm(int userProfileId)
            : this(new GetUserAccessList(), new GetActualUserAccessList(), new GetActualUserBranchList(), new DeleteUserProfile())
        {

            InitializeComponent();
            UserProperties.selectedUserProfileID = userProfileId;
            UserProperties.userType = userProfileId == 0 ? userTypeEnum.NewUser : userTypeEnum.ExistingUser;

        }

        public UserProfileDetailsForm(IUserProfile<DataTable> getUserAccessList, IUserProfile<DataTable> getActualUserAccessList, IUserProfile<DataTable> getActualUserBranchList, IUserProfile<int> deleteUserProfile)
        {
            this.getUserAccessList = getUserAccessList;
            this.getActualUserAccessList = getActualUserAccessList;
            this.getActualUserBranchList = getActualUserBranchList;
            this.deleteUserProfile = deleteUserProfile;
        }

        private void UserProfileDetails_Load(object sender, EventArgs e)
        {
            comboBoxesInForm = pnlSystemBranch.Controls.OfType<ComboBox>();
            chechBoxesInForm = pnlSystemBranch.Controls.OfType<CheckBox>();
            FillPermissionComboboxes();
            if (UserProperties.userType == userTypeEnum.ExistingUser)
            {
                FillDataFromDataBase();
                btnAddNewUser.Visible = false;
            }
            else
                groupBoxFooter.Visible = false;
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            bool isallValidated = checkValidations();
            if (isallValidated)
            {
                var confirmResult = MessageBox.Show("Are you sure to save this item?", "Confirm Update!!", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    UpdateEntredData();
                }

            }
        }


        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this item?",
                                    "Confirm Delete!!",
                                    MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                DeleteUserProfile();
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to cancel changes?",
                                  "Confirm Cancel!!",
                                  MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                FillPermissionComboboxes();
                FillDataFromDataBase();
            }

        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            CloseApplication();
        }

        void CloseApplication()
        {
            this.Hide();
            UserListForm uf = new UserListForm();
            uf.Show();
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            AddNewUser();
        }

    }
}
