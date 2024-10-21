using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnnecessaryManufactureProductRelationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            // migrationBuilder.CreateTable(
            //     name: "Categories",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //         Type = table.Column<bool>(type: "boolean", nullable: false),
            //         ParentCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
            //         CategoryId = table.Column<Guid>(type: "uuid", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Categories", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Categories_Categories_CategoryId",
            //             column: x => x.CategoryId,
            //             principalTable: "Categories",
            //             principalColumn: "Id");
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "Invoices",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         OwnerFirstName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //         OwnerLastName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //         IdentificationCode = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //         IssuerName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //         IssueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         PaymentDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //         TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
            //         PaymentStatus = table.Column<string>(type: "payment_status", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Invoices", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Manufacturers",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         OwnerName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //         ManufacturerCountry = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
            //         Email = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
            //         Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //         PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
            //         Rate = table.Column<int>(type: "integer", nullable: false),
            //         EstablishDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         Status = table.Column<bool>(type: "boolean", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Manufacturers", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Roles",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Roles", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Users",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         FirstName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //         LastName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //         Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
            //         PasswordHash = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: true),
            //         Email = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
            //         PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //         RoleId = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Users", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Products",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
            //         Price = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
            //         Inventory = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
            //         Dop = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         Doe = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         Status = table.Column<bool>(type: "boolean", nullable: false),
            //         InvoiceId = table.Column<Guid>(type: "uuid", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Products", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Products_Invoices_InvoiceId",
            //             column: x => x.InvoiceId,
            //             principalTable: "Invoices",
            //             principalColumn: "Id");
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ManufacturerProduct2",
            //     columns: table => new
            //     {
            //         Manufacturers2Id = table.Column<Guid>(type: "uuid", nullable: false),
            //         Products2Id = table.Column<Guid>(type: "uuid", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ManufacturerProduct2", x => new { x.Manufacturers2Id, x.Products2Id });
            //         table.ForeignKey(
            //             name: "FK_ManufacturerProduct2_Manufacturers_Manufacturers2Id",
            //             column: x => x.Manufacturers2Id,
            //             principalTable: "Manufacturers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ManufacturerProduct2_Products_Products2Id",
            //             column: x => x.Products2Id,
            //             principalTable: "Products",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "ProductInvoices",
            //     columns: table => new
            //     {
            //         ProductId = table.Column<Guid>(type: "uuid", nullable: false),
            //         InvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
            //         Count = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ProductInvoices", x => new { x.ProductId, x.InvoiceId });
            //         table.ForeignKey(
            //             name: "FK_ProductInvoices_Invoices_InvoiceId",
            //             column: x => x.InvoiceId,
            //             principalTable: "Invoices",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_ProductInvoices_Products_ProductId",
            //             column: x => x.ProductId,
            //             principalTable: "Products",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Categories_CategoryId",
            //     table: "Categories",
            //     column: "CategoryId");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Invoices_IdentificationCode",
            //     table: "Invoices",
            //     column: "IdentificationCode",
            //     unique: true);
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ManufacturerProduct2_Products2Id",
            //     table: "ManufacturerProduct2",
            //     column: "Products2Id");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Manufacturers_Address",
            //     table: "Manufacturers",
            //     column: "Address",
            //     unique: true);
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Manufacturers_Email",
            //     table: "Manufacturers",
            //     column: "Email",
            //     unique: true);
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Manufacturers_PhoneNumber",
            //     table: "Manufacturers",
            //     column: "PhoneNumber",
            //     unique: true);
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_ProductInvoices_InvoiceId",
            //     table: "ProductInvoices",
            //     column: "InvoiceId");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Products_InvoiceId",
            //     table: "Products",
            //     column: "InvoiceId");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Roles_Name",
            //     table: "Roles",
            //     column: "Name",
            //     unique: true);
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Users_Email",
            //     table: "Users",
            //     column: "Email",
            //     unique: true);
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Users_Username",
            //     table: "Users",
            //     column: "Username",
            //     unique: true);

            migrationBuilder.DropTable("ManufacturerProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ManufacturerProduct2");

            migrationBuilder.DropTable(
                name: "ProductInvoices");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
