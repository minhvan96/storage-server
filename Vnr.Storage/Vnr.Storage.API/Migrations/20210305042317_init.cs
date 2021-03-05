using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vnr.Storage.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EncryptedFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Path = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    FullPath = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncryptedFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RijndaelKeys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    IV = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
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
                name: "EncryptedFiles");

            migrationBuilder.DropTable(
                name: "RijndaelKeys");
        }
    }
}
