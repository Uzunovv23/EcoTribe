using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoTribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunteerIdToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VolunteerId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VolunteerId",
                table: "AspNetUsers");
        }
    }
}
