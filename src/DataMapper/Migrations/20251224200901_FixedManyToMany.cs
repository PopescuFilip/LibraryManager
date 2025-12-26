using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMapper.Migrations
{
    /// <inheritdoc />
    public partial class FixedManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BorrowRecords_BorrowRecordId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BorrowRecordId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowRecordId",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookBorrowRecord",
                columns: table => new
                {
                    BorrowRecordId = table.Column<int>(type: "int", nullable: false),
                    BorrowedBooksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowRecord", x => new { x.BorrowRecordId, x.BorrowedBooksId });
                    table.ForeignKey(
                        name: "FK_BookBorrowRecord_Books_BorrowedBooksId",
                        column: x => x.BorrowedBooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBorrowRecord_BorrowRecords_BorrowRecordId",
                        column: x => x.BorrowRecordId,
                        principalTable: "BorrowRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowRecord_BorrowedBooksId",
                table: "BookBorrowRecord",
                column: "BorrowedBooksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBorrowRecord");

            migrationBuilder.AddColumn<int>(
                name: "BorrowRecordId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BorrowRecordId",
                table: "Books",
                column: "BorrowRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BorrowRecords_BorrowRecordId",
                table: "Books",
                column: "BorrowRecordId",
                principalTable: "BorrowRecords",
                principalColumn: "Id");
        }
    }
}
