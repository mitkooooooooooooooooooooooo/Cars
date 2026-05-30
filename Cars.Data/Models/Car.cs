using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cars.Data.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Make { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Model { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Year { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        public bool IsSold { get; set; }

        public int? DealerId { get; set; }

        [ForeignKey(nameof(DealerId))]
        public virtual Dealer? Dealer { get; set; }
    }
}