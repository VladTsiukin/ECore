using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECore.Web.Models.ViewModels
{
    public class CardVM
    {
        
        public int Id { get; set; }
        [Required]
        public long Interval { get; set; }
        [Required]
        public int UserScore { get; set; }
        [Required]
        public int GroupRepetition { get; set; }
        [Required]
        public DateTimeOffset DtoInterval { get; set; }
        [Required]
        public double Efactor { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        public ItemVM Item { get; set; }

    }
}
