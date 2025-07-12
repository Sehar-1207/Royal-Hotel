namespace RoyalHillHotel.Models
{
    public class InvoiceDto
    {
            public int BookingId { get; set; }
            public decimal ExtraCharges { get; set; }
            public bool IsPaid { get; set; }
    }
}
