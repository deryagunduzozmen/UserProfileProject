using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserProfileProject.DAL.Interfaces
{
    public interface IUserProfile<T1>
    {
        T1 Manage();
    }
}
