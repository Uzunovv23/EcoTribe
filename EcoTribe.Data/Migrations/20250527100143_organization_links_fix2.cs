using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoTribe.Data.Migrations
{
    /// <inheritdoc />
    public partial class organization_links_fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganization_AspNetUsers_UserId",
                table: "UserOrganization");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganization_Organizations_OrganizationId",
                table: "UserOrganization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOrganization",
                table: "UserOrganization");

            migrationBuilder.RenameTable(
                name: "UserOrganization",
                newName: "UserOrganizations");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrganization_OrganizationId",
                table: "UserOrganizations",
                newName: "IX_UserOrganizations_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOrganizations",
                table: "UserOrganizations",
                columns: new[] { "UserId", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizations_AspNetUsers_UserId",
                table: "UserOrganizations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizations_Organizations_OrganizationId",
                table: "UserOrganizations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizations_AspNetUsers_UserId",
                table: "UserOrganizations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizations_Organizations_OrganizationId",
                table: "UserOrganizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOrganizations",
                table: "UserOrganizations");

            migrationBuilder.RenameTable(
                name: "UserOrganizations",
                newName: "UserOrganization");

            migrationBuilder.RenameIndex(
                name: "IX_UserOrganizations_OrganizationId",
                table: "UserOrganization",
                newName: "IX_UserOrganization_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOrganization",
                table: "UserOrganization",
                columns: new[] { "UserId", "OrganizationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganization_AspNetUsers_UserId",
                table: "UserOrganization",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganization_Organizations_OrganizationId",
                table: "UserOrganization",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
