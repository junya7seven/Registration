using Dapper;
using DataSource.Model;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace DataSource
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(string connectionString, ILogger<UserRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task Insert(User model)
        {
            try
            {
                
                using(var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var sql = @"INSERT INTO userinfo (login, hashpassword, email, age, country, city, phone, gender, role, user_date) VALUES (@Login, @HashPassword, @Email, @Age, @Country, @City, @Phone, @Gender, @Role, @DateTime)";
                    await connection.ExecuteAsync(sql, model);
                    _logger.LogInformation($"User: {model.Login} has been added");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error insert user into the database");
                throw;
            }
        }
        public async Task<bool> SearchByLoginAndValues(User user)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var sql = "SELECT COUNT(1) FROM userinfo WHERE login = @login OR email = @Email";
                    var count = await connection.ExecuteScalarAsync<int>(sql, new { user.Login, user.Email });
                    return count > 0;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error searching user in the database");
                throw;
            }
        }

        public async Task<User> SearchByLogin(string login)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var sql = "SELECT login,hashpassword FROM userinfo WHERE login = @Login";
                    return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Login = login });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error getting user from the database");
                throw;
            }
        }

        public async Task Delete(User user)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = "DELETE FROM userinfo WHERE login = @login";
                    await connection.ExecuteAsync(sql, new { user.Login });
                    _logger.LogInformation($"User: {user.Login} has been deleted");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error deleting user from the database");
                throw;
            }
        }

        public async Task Update()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    var sql = "";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM users";
                return await db.QueryAsync<User>(sqlQuery);
            }
        }
    }
}