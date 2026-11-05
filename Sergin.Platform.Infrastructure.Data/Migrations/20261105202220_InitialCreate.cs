using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sergin.Platform.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.EnsureSchema(name: "pf");

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // The schema stays: the migrations history table this migration is recorded in lives there.
    }
}
