using DataSource.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserChangeService
    {
        Task<bool> UserRaname(string newLogin);
        Task<bool> UserChangePassword(string newPassword);
    }
}
