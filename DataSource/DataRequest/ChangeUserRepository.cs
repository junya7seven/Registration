using Dapper;
using DataSource.DataRequest.Interface;
using DataSource.Model;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace DataSource.DataRequest
{
    public class ChangeUserRepository : UserRepository, IChangeUserRepository
    {
        public ChangeUserRepository(string connectionString, ILogger<UserRepository> logger)
            : base(connectionString, logger) { }

        public async Task<bool> RenameUser(User model, string newName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var sql = "UPDATE userinfo SET login = @newName WHERE login = @Login";
                    var result = await connection.ExecuteAsync(sql, new { newName, model.Login });
                    _logger.LogInformation($"User: {model.Login} changed login to {newName}");

                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error changing user login in the database");
                throw;
            }
        }

        public async Task<bool> ChangePassword(User model, string newPassword)
        {
            try
            {
                if (BCrypt.Net.BCrypt.Verify(newPassword, model.HashPassword))
                {
                    _logger.LogInformation("The new password matches the old password.");
                    return false;
                }

                var hashedPassword = PasswordHash.HashPassword(newPassword);
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var sql = "UPDATE userinfo SET hashpassword = @hashedPassword WHERE login = @Login";
                    var result = await connection.ExecuteAsync(sql, new { hashedPassword, model.Login });
                    _logger.LogInformation($"User: {model.Login} changed password");

                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error changing user password in the database");
                throw;
            }
        }
    }
}