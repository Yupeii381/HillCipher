using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HillCipher.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEmailAndCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "action",
                table: "RequestHistories",
                newName: "Action");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "RequestHistories",
                newName: "action");
        }
    }
}
