using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoTribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class EventVolunteer_ApplicationUserId_toInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventVolunteers_AspNetUsers_ApplicationUserId",
                table: "EventVolunteers");

            migrationBuilder.DropIndex(
                name: "IX_EventVolunteers_ApplicationUserId",
                table: "EventVolunteers");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "EventVolunteers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "EventVolunteers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventVolunteers_ApplicationUserId1",
                table: "EventVolunteers",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EventVolunteers_AspNetUsers_ApplicationUserId1",
                table: "EventVolunteers",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventVolunteers_AspNetUsers_ApplicationUserId1",
                table: "EventVolunteers");

            migrationBuilder.DropIndex(
                name: "IX_EventVolunteers_ApplicationUserId1",
                table: "EventVolunteers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "EventVolunteers");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "EventVolunteers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_EventVolunteers_ApplicationUserId",
                table: "EventVolunteers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventVolunteers_AspNetUsers_ApplicationUserId",
                table: "EventVolunteers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
