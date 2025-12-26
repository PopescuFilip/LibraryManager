using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMapper.Migrations
{
    /// <inheritdoc />
    public partial class Cleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions");

            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "BookDomain");

            migrationBuilder.DropTable(
                name: "BookRecords");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "BookEditions",
                newName: "BookDefinitionId");

            migrationBuilder.RenameIndex(
                name: "IX_BookEditions_BookId",
                table: "BookEditions",
                newName: "IX_BookEditions_BookDefinitionId");

            migrationBuilder.AddColumn<int>(
                name: "BookEditionId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BorrowRecordId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BookDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorBookDefinition",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    WrittenBooksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBookDefinition", x => new { x.AuthorsId, x.WrittenBooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBookDefinition_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBookDefinition_BookDefinitions_WrittenBooksId",
                        column: x => x.WrittenBooksId,
                        principalTable: "BookDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookDefinitionDomain",
                columns: table => new
                {
                    BookDefinitionId = table.Column<int>(type: "int", nullable: false),
                    DomainsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDefinitionDomain", x => new { x.BookDefinitionId, x.DomainsId });
                    table.ForeignKey(
                        name: "FK_BookDefinitionDomain_BookDefinitions_BookDefinitionId",
                        column: x => x.BookDefinitionId,
                        principalTable: "BookDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookDefinitionDomain_Domains_DomainsId",
                        column: x => x.DomainsId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookEditionId",
                table: "Books",
                column: "BookEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BorrowRecordId",
                table: "Books",
                column: "BorrowRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBookDefinition_WrittenBooksId",
                table: "AuthorBookDefinition",
                column: "WrittenBooksId");

            migrationBuilder.CreateIndex(
                name: "IX_BookDefinitionDomain_DomainsId",
                table: "BookDefinitionDomain",
                column: "DomainsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_BookDefinitions_BookDefinitionId",
                table: "BookEditions",
                column: "BookDefinitionId",
                principalTable: "BookDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookEditions_BookEditionId",
                table: "Books",
                column: "BookEditionId",
                principalTable: "BookEditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BorrowRecords_BorrowRecordId",
                table: "Books",
                column: "BorrowRecordId",
                principalTable: "BorrowRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookEditions_BookDefinitions_BookDefinitionId",
                table: "BookEditions");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookEditions_BookEditionId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_BorrowRecords_BorrowRecordId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "AuthorBookDefinition");

            migrationBuilder.DropTable(
                name: "BookDefinitionDomain");

            migrationBuilder.DropTable(
                name: "BookDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookEditionId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BorrowRecordId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookEditionId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowRecordId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "BookDefinitionId",
                table: "BookEditions",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookEditions_BookDefinitionId",
                table: "BookEditions",
                newName: "IX_BookEditions_BookId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    BooksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookDomain",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    DomainsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDomain", x => new { x.BooksId, x.DomainsId });
                    table.ForeignKey(
                        name: "FK_BookDomain_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookDomain_Domains_DomainsId",
                        column: x => x.DomainsId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookEditionId = table.Column<int>(type: "int", nullable: false),
                    BorrowRecordId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookRecords_BookEditions_BookEditionId",
                        column: x => x.BookEditionId,
                        principalTable: "BookEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRecords_BorrowRecords_BorrowRecordId",
                        column: x => x.BorrowRecordId,
                        principalTable: "BorrowRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksId",
                table: "AuthorBook",
                column: "BooksId");

            migrationBuilder.CreateIndex(
                name: "IX_BookDomain_DomainsId",
                table: "BookDomain",
                column: "DomainsId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_BookEditionId",
                table: "BookRecords",
                column: "BookEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRecords_BorrowRecordId",
                table: "BookRecords",
                column: "BorrowRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookEditions_Books_BookId",
                table: "BookEditions",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
