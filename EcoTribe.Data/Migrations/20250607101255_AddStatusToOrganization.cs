using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoTribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Organizations");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Organizations");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Organizations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
