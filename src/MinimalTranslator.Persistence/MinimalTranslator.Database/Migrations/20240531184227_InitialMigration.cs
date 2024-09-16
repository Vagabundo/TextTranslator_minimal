using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalTranslator.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageTo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OriginalText = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    LanguageFrom = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TranslatedText = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => new { x.Id, x.LanguageTo });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translations");
        }
    }
}
