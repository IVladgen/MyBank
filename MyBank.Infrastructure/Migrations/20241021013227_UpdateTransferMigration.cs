using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransferMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_UserId1",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_AccountId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AccountId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId1",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AccountFromId",
                table: "Transfers",
                column: "AccountFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AccountToId",
                table: "Transfers",
                column: "AccountToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_AccountFromId",
                table: "Transfers",
                column: "AccountFromId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_AccountToId",
                table: "Transfers",
                column: "AccountToId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_AccountFromId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_AccountToId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AccountFromId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AccountToId",
                table: "Transfers");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Transfers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Accounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AccountId",
                table: "Transfers",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId1",
                table: "Accounts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_UserId1",
                table: "Accounts",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_AccountId",
                table: "Transfers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
