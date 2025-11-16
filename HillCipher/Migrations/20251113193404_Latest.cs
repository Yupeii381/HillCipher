using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HillCipher.Migrations
{
    /// <inheritdoc />
    public partial class Latest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestHistories_Users_UserId",
                table: "RequestHistories");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RequestHistories",
                newName: "Timestamp");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Texts",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(600)",
                oldMaxLength: 600);

            migrationBuilder.AlterColumn<string>(
                name: "ResultText",
                table: "RequestHistories",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InputText",
                table: "RequestHistories",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "RequestHistories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ActionName",
                table: "RequestHistories",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "RequestHistories",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "RequestHistories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserEntityId",
                table: "RequestHistories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_Timestamp",
                table: "RequestHistories",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_UserEntityId",
                table: "RequestHistories",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestHistories_Users_UserEntityId",
                table: "RequestHistories",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestHistories_Users_UserEntityId",
                table: "RequestHistories");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_RequestHistories_Timestamp",
                table: "RequestHistories");

            migrationBuilder.DropIndex(
                name: "IX_RequestHistories_UserEntityId",
                table: "RequestHistories");

            migrationBuilder.DropColumn(
                name: "ActionName",
                table: "RequestHistories");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "RequestHistories");

            migrationBuilder.DropColumn(
                name: "Success",
                table: "RequestHistories");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "RequestHistories");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "RequestHistories",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Texts",
                type: "character varying(600)",
                maxLength: 600,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "ResultText",
                table: "RequestHistories",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InputText",
                table: "RequestHistories",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "RequestHistories",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestHistories_Users_UserId",
                table: "RequestHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
