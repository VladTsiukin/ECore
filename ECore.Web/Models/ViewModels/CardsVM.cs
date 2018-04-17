using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECore.Web.Models.ViewModels
{
    public class CardsVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название коллекции!")]
        [MaxLength(50, ErrorMessage = "Можно ввести не более 50-ти символов!")]
        public string Name { get; set; }
    }
}
