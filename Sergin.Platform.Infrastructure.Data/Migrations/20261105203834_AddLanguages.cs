using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sergin.Platform.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddLanguages : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "pf");

        migrationBuilder.CreateTable(
            name: "language",
            schema: "pf",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                culture = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                direction = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<Guid>(type: "uuid", nullable: false),
                modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                row_version = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_language", x => x.id);
            });

        migrationBuilder.CreateIndex(
            name: "ix_language_culture",
            schema: "pf",
            table: "language",
            column: "culture",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "language",
            schema: "pf");
    }
}
