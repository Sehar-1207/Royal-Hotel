using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoyalHillHotel.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomStatusConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Room_Status",
                table: "Rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Room_Status",
                table: "Rooms",
                sql: "Status IN ('Available', 'Occupied')");
        }
    }
}
