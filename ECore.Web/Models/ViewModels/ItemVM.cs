using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECore.Web.Models.ViewModels
{
    public class ItemVM
    {
        [Required(ErrorMessage = "Пожалуйста, введите желаемый вопрос!")]
        [MaxLength(250, ErrorMessage = "Можно ввести не более 250-ти символов!")]
        public string Face { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите желаемый ответ!")]
        [MaxLength(250, ErrorMessage = "Можно ввести не более 250-ти символов!")]
        public string Back { get; set; }

    }
}
