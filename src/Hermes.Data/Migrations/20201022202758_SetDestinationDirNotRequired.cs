using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes.Data.Migrations
{
    public partial class SetDestinationDirNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DestinationDir",
                table: "Mail",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DestinationDir",
                table: "Mail",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048,
                oldNullable: true);
        }
    }
}
