using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMapper.Migrations
{
    /// <inheritdoc />
    public partial class BorrowedByProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowedById",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BorrowedById",
                table: "Books",
                column: "BorrowedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Clients_BorrowedById",
                table: "Books",
                column: "BorrowedById",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Clients_BorrowedById",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BorrowedById",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowedById",
                table: "Books");
        }
    }
}
