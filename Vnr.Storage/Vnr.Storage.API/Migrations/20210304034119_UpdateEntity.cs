using Microsoft.EntityFrameworkCore.Migrations;

namespace Vnr.Storage.API.Migrations
{
    public partial class UpdateEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "EncryptedFiles",
                type: "TEXT",
                maxLength: 512,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "EncryptedFiles");
        }
    }
}
