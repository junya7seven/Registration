using DataSource.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.DataRequest.Interface
{
    public interface IChangeUserRepository
    {
        Task<bool> RenameUser(User model, string newName);
        Task<bool> ChangePassword(User model, string newName);

    }
}
