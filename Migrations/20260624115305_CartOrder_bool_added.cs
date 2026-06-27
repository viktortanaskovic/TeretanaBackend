using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeretanaBackend.Migrations
{
    /// <inheritdoc />
    public partial class CartOrder_bool_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OrderMade",
                table: "Carts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderMade",
                table: "Carts");
        }
    }
}
