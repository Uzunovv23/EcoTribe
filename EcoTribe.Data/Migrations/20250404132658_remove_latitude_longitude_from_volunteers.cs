using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoTribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class remove_latitude_longitude_from_volunteers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Volunteers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Volunteers",
                type: "numeric(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Volunteers",
                type: "numeric(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
