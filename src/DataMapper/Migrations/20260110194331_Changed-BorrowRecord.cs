using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMapper.Migrations
{
    /// <inheritdoc />
    public partial class ChangedBorrowRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBorrowRecord");

            migrationBuilder.AddColumn<int>(
                name: "BorrowedBookId",
                table: "BorrowRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowedUntil",
                table: "BorrowRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRecords_BorrowedBookId",
                table: "BorrowRecords",
                column: "BorrowedBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRecords_Books_BorrowedBookId",
                table: "BorrowRecords",
                column: "BorrowedBookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRecords_Books_BorrowedBookId",
                table: "BorrowRecords");

            migrationBuilder.DropIndex(
                name: "IX_BorrowRecords_BorrowedBookId",
                table: "BorrowRecords");

            migrationBuilder.DropColumn(
                name: "BorrowedBookId",
                table: "BorrowRecords");

            migrationBuilder.DropColumn(
                name: "BorrowedUntil",
                table: "BorrowRecords");

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
    }
}
