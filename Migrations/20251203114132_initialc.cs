using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Migrations
{
    /// <inheritdoc />
    public partial class initialc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TicketMs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMs_UserId",
                table: "TicketMs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketMs_AspNetUsers_UserId",
                table: "TicketMs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketMs_AspNetUsers_UserId",
                table: "TicketMs");

            migrationBuilder.DropIndex(
                name: "IX_TicketMs_UserId",
                table: "TicketMs");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TicketMs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
