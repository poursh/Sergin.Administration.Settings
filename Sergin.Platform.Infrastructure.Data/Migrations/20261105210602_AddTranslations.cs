using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sergin.Platform.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddTranslations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "translation",
            schema: "pf",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                language_id = table.Column<Guid>(type: "uuid", nullable: false),
                key = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                source_text = table.Column<string>(type: "text", nullable: false),
                created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_by = table.Column<Guid>(type: "uuid", nullable: false),
                modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                row_version = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_translation", x => x.id);
                table.ForeignKey(
                    name: "fk_translation_language_language_id",
                    column: x => x.language_id,
                    principalSchema: "pf",
                    principalTable: "language",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_translation_language_id_key",
            schema: "pf",
            table: "translation",
            columns: ["language_id", "key"],
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "translation",
            schema: "pf");
    }
}
