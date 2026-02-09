using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Question = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Answer = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IterationCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PriceCurrency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "Name", "UpdatedAt", "PriceAmount", "PriceCurrency" },
                values: new object[,]
                {
                    { new Guid("1ac2d2c4-8e26-4d1f-9a0a-93db3f5e2b79"), 7, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Smycz zwijana dla psów do 50kg", "Flexi Smycz automatyczna 8m", null, 119m, "PLN" },
                    { new Guid("2b5a69fd-4d44-4c32-bf5d-7f6b2a5d7f31"), 2, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Karma dla kociąt do 12 miesiąca życia", "Brit Premium Kitten 8kg", null, 159m, "PLN" },
                    { new Guid("3a7c1ad6-6fb4-4f9a-9a2d-7c11df74e6ff"), 2, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Sucha karma dla dorosłych kotów z kurczakiem", "Whiskas Adult Kurczak 7kg", null, 129m, "PLN" },
                    { new Guid("4c6f2a1b-5e7a-45ad-8b2f-63e2c9b1a7a4"), 3, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Kompletny zestaw CO2 dla roślin akwariowych", "JBL ProFlora CO2 Set", null, 549m, "PLN" },
                    { new Guid("4e2f0a6b-8c17-4c65-96f1-1d7f0b5a2c3e"), 5, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Wytrzymała zabawka do napełniania smakołykami", "Kong Classic Large", null, 69m, "PLN" },
                    { new Guid("5ad1d7e0-6d9c-4c0c-bf9a-0d84e2b2f4ef"), 6, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Naturalne siano łąkowe, podstawa diety", "Vitapol Siano dla królików 1kg", null, 25m, "PLN" },
                    { new Guid("6bf43c6c-2c24-4d82-9c2f-05a1e7c49a06"), 1, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Premium karma dla dorosłych psów średnich ras", "Royal Canin Adult Dog 15kg", null, 289m, "PLN" },
                    { new Guid("9b1f4d62-9f6e-4b0e-8f42-4b2c7c4e2e6a"), 6, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Klatka 60x40cm z wyposażeniem", "Ferplast Klatka dla chomika", null, 189m, "PLN" },
                    { new Guid("e3fa1b9b-3758-46f1-8f3a-5f0c9a2a0e9e"), 3, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Uzdatniacz wody do akwarium, neutralizuje chlor", "Tetra AquaSafe 500ml", null, 45m, "PLN" },
                    { new Guid("f2a39b45-1f77-4c0b-a4b1-8a6b5d2c9a18"), 4, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Wysoki drapak z platformami i domkiem", "Trixie Drapak XL 150cm", null, 399m, "PLN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
