using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalHillHotel.Models
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public int Duration { get; set; } 
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}
