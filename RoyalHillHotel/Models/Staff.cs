using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalHillHotel.Models
{
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; } // Manager, Receptionist, etc.
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public DateTime DateJoined { get; set; }
        [Required]
        public string Shift { get; set; } // Morning, Evening, Night
        [Required]
        public decimal Salary { get; set; }
    }
}
