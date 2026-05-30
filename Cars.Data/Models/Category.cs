using System.ComponentModel.DataAnnotations;

namespace Cars.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}