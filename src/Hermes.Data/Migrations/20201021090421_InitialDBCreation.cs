using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes.Data.Migrations
{
    public partial class InitialDBCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Filename = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "text", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: true),
                    DestinationDir = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mail", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mail");
        }
    }
}
