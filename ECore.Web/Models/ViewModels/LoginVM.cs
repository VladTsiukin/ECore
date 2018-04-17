using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECore.Web.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";

    }
}
