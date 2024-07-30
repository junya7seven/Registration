using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public static class PasswordHash
    {
        public static string HashPassword(string password)
        {
            string hashPas = BCrypt.Net.BCrypt.HashPassword(password);
            return hashPas;
        }
    }
}
