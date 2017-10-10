using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfileProject.Common
{
    public static class UserProperties
    {
        public static int selectedUserProfileID { get; set; }
        public static userTypeEnum userType { get; set; }
    }
    public enum userTypeEnum{NewUser,ExistingUser};
}
