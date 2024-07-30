using DataSource.Model;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterModel model);
        Task<string> AuthenticateUser(LoginModel model);
        string GenerateJwtToken(User user);
        Task<bool> UserValidation(RegisterModel model);
    }
}
