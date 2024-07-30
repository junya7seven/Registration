using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource.Model
{
    public class User
    {
        public string Login { get; set; }

        public string HashPassword { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

        public Role Role { get; set; }

        public DateTime DateTime { get; set; }

    }
    public enum Role
    {
        User,
        Admin 
    }
}
