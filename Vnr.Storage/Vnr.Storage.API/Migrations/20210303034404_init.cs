using Microsoft.EntityFrameworkCore.Migrations;

namespace Vnr.Storage.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RijndaelKeys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    IV = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RijndaelKeys", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RijndaelKeys",
                columns: new[] { "Id", "IV", "Key" },
                values: new object[] { 1L, "QtgaRmKYbpvV4cPp3DtU/g==", "fjc0nLMh5acJhSz5XjN4X6zExZUShzYN11Twf6VSwFE=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RijndaelKeys");
        }
    }
}
