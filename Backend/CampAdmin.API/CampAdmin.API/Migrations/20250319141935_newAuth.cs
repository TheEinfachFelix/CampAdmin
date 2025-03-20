using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampAdmin.API.Migrations
{
    /// <inheritdoc />
    public partial class newAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Permissions",
                table: "ApiKeys",
                newName: "Roles");

            migrationBuilder.AddColumn<Guid>(
                name: "ApiKeyId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApiKeyId",
                table: "Users",
                column: "ApiKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ApiKeys_ApiKeyId",
                table: "Users",
                column: "ApiKeyId",
                principalTable: "ApiKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ApiKeys_ApiKeyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ApiKeyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApiKeyId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "ApiKeys",
                newName: "Permissions");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
