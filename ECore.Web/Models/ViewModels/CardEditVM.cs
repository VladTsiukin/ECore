using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECore.Web.Models.ViewModels
{
    public class CardEditVM
    {
        [Required]
        public int Id { get; set; }

        public int UserScore { get; set; }

        public double Efactor { get; set; }

        [Required]
        public int CardsId { get; set; }
        [Required]
        public ItemVM Item { get; set; }
    }
}

