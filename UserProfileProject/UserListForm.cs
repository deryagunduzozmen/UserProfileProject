using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserProfileProject.Common;
using UserProfileProject.DAL.BO;
using UserProfileProject.DAL.Interfaces;


namespace UserProfileProject
{
    public partial class UserListForm : Form
    {
        private readonly IUserProfile<DataTable> getUserList;
        public UserListForm()
            : this(new GetUserList())
        {
            InitializeComponent();
        }

        public UserListForm(IUserProfile<DataTable> getUserList)
        {
            this.getUserList = getUserList;
        }
        private void UserListForm_Load(object sender, EventArgs e)
        {
            DataTable dt = getUserList.Manage();
            dataGridViewUserList.DataSource = dt;
            if (dt.Rows.Count > 0)
                UserProperties.selectedUserProfileID = 1;
        }

        private void btn_ShowUserProfileDetails_Click(object sender, EventArgs e)
        {
            ShowUserProfileForm(UserProperties.selectedUserProfileID);
        }

        //if new user send 0 for userprofileId
        private void ShowUserProfileForm(int userProfileId)
        {
            UserProfileDetailsForm userProfile = new UserProfileDetailsForm(userProfileId);
            this.Hide();
            userProfile.ShowDialog();
        }

        private void btnAddNewUserProfile_Click(object sender, EventArgs e)
        {
            ShowUserProfileForm(0);
        }


        private void dataGridViewUserList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UserProperties.selectedUserProfileID = Convert.ToInt32(dataGridViewUserList.Rows[e.RowIndex].Cells[0].Value);
        }
    }
}

