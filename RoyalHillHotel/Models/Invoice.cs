using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalHillHotel.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal RoomCharges { get; set; }
        public decimal ExtraCharges { get; set; }
        public double TotalAmount { get; set; } 
        public bool IsPaid { get; set; }
    }

}
