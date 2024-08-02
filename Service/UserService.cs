using DataSource;
using DataSource.DataRequest;
using DataSource.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Service.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUser(RegisterModel model)
        {
            try
            {
                var user = new User
                {
                    Login = model.Login,
                    HashPassword = PasswordHash.HashPassword(model.Password),
                    Email = model.Email,
                    Age = model.Age,
                    Country = model.Country,
                    City = model.City,
                    Phone = model.Phone,
                    Gender = model.Gender,
                    Role = Role.User,
                    DateTime = DateTime.Now
                };
                var existUser = await _userRepository.SearchByLoginAndValues(user);
                if (existUser)
                {
                    _logger.LogInformation($"User: {model.Login} already exist");
                    return false;
                }
                await _userRepository.Insert(user);
                _logger.LogInformation($"User: {model.Login} registered successfully");
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,"Failed to connect to the database");
                throw;
            }
        }
        public async Task<string> AuthenticateUser(LoginModel model)
        {
            var user = await _userRepository.SearchByLogin(model.Login);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.HashPassword))
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public async Task<bool> UserValidation(RegisterModel model)
        {
            var result = new List<ValidationResult>();
            var context = new ValidationContext(model);

            if (Validator.TryValidateObject(model, context, result, true)) return true;
            else return false;
        }
    }
}
