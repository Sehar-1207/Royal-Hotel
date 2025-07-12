using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalHillHotel.Models
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RoomNo { get; set; }
        public string RoomType { get; set; } // single bouble suite
        public string Status { get; set; } // "Available" or "Occupied"
        public decimal Price { get; set; }
    }
}
