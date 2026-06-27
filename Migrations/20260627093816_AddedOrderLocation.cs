using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeretanaBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationSend",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationSend",
                table: "Orders");
        }
    }
}
