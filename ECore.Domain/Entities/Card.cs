using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECore.Domain.Entities
{
    /// <summary>
    /// Card entity.
    /// </summary>
    [Table("Card")]
    public class Card
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CardsId { get; set; }
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
        [Required]
        public int ItemId { get; set; }
        
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        [ForeignKey("CardsId")]
        public virtual CardsCollection CardsCollection { get; set; }
    }
}
