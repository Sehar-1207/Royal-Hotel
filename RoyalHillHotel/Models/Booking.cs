using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalHillHotel.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int Duration { get; set; } // in days
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }

    }
}
