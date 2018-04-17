using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECore.Domain.Entities
{
    /// <summary>
    /// Entity cards collection.
    /// </summary>
    [Table("CardsCollection")]
    public class CardsCollection
    {

        public CardsCollection()
        {
            Cards = new HashSet<Card>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string AppUserId { get; set; }
        [Required]
        public DateTimeOffset DateOfCreation { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите название коллекции!")]
        [MaxLength(50, ErrorMessage = "Можно ввести не более 50-ти символов!")]
        public string Name { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }
        public virtual  ICollection<Card> Cards{ get; set; }

    }
}
