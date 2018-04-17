using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECore.Domain.Entities
{
    /// <summary>
    /// Item - question/answer (face/back card).
    /// </summary>
    [Table("Item")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(1000, ErrorMessage = "Можно ввести не более 1000 символов!")]
        public string Face { get; set; }
        [MaxLength(1000, ErrorMessage = "Можно ввести не более 1000 символов!")]
        public string Back { get; set; }

        public virtual Card Card { get; set; }
    }
}
