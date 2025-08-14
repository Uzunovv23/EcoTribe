using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoTribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailConrirmationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmationTokenExpiry",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationTokenExpiry",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "AspNetUsers");
        }
    }
}
