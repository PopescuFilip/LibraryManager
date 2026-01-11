using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMapper.Migrations
{
    /// <inheritdoc />
    public partial class AddedRequestedByProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestedById",
                table: "Extensions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Extensions_RequestedById",
                table: "Extensions",
                column: "RequestedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Extensions_Clients_RequestedById",
                table: "Extensions",
                column: "RequestedById",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Extensions_Clients_RequestedById",
                table: "Extensions");

            migrationBuilder.DropIndex(
                name: "IX_Extensions_RequestedById",
                table: "Extensions");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "Extensions");
        }
    }
}
