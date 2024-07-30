using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Service.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Поле не заполнено")]
        [StringLength(128,ErrorMessage = "Слишком длинный логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Длина пароля должна быть от 8 до 32")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Пароли не совподают")]
        [Compare("Password")]
        public string Confpassword { get; set; }
        
        [Required(ErrorMessage = "Поле не заполнено")]
        [EmailAddress(ErrorMessage = "Неверно введены данные")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        [Range(1, 110,ErrorMessage = "Неверный диапазон от 1 до 110")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string City { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        [Phone(ErrorMessage = "Неверный формат номера")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Gender { get; set; }

    }
}
