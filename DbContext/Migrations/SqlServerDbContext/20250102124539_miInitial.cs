using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class miInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "supusr");

            migrationBuilder.CreateTable(
                name: "Zoos",
                schema: "supusr",
                columns: table => new
                {
                    ZooId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zoos", x => x.ZooId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zoos",
                schema: "supusr");
        }
    }
}
