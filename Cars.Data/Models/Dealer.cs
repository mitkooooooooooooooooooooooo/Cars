using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Cars.Data.Models
{
    public class Dealer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = null!;

        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual IdentityUser? User { get; set; }

        public virtual ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}