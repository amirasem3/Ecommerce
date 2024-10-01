using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintsForManifacturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Manufacturers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Manufacturers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Manufacturers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Address",
                table: "Manufacturers",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Email",
                table: "Manufacturers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_PhoneNumber",
                table: "Manufacturers",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_Address",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_Email",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_PhoneNumber",
                table: "Manufacturers");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Manufacturers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Manufacturers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Manufacturers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
