using DataSource;
using DataSource.DataRequest.Interface;
using DataSource.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserChangeService : IUserChangeService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserChangeService> _logger;
        private readonly IChangeUserRepository _changeRepository;

        public UserChangeService(IUserRepository userRepository, ILogger<UserChangeService> logger, IChangeUserRepository changeRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _changeRepository = changeRepository;
	
        }

        public async Task<bool> UserRaname(string newLogin)
        {
            try
            {
                var user =  await _userRepository.SearchByLogin(newLogin);
                if (user.Login == newLogin)
                {
                    _logger.LogInformation($"User {user.Login} failed to change login to {newLogin} ");
                    return false;
                }
                var res = await _changeRepository.RenameUser(user,newLogin);
                if (res)
                {
                    _logger.LogInformation($"User {user.Login} change login to {newLogin} ");
                    return true;
                }
                _logger.LogInformation($"User {user.Login} failed to change login to {newLogin} ");
                return false;

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "failed to connect database");
                throw;
            }
        }

        public async Task<bool> UserChangePassword(string newPassword)
        {
            return true;
        }

    }
}
