using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vnr.Storage.API.Migrations
{
    public partial class addaeskey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AesKey",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<byte[]>(type: "BLOB", maxLength: 40, nullable: true),
                    IV = table.Column<byte[]>(type: "BLOB", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AesKey", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AesKey",
                columns: new[] { "Id", "IV", "Key" },
                values: new object[] { 1L, new byte[] { 47, 229, 196, 4, 91, 184, 189, 79, 12, 248, 225, 192, 253, 28, 118, 211 }, new byte[] { 231, 188, 153, 49, 147, 194, 81, 240, 141, 247, 144, 249, 104, 36, 170, 136, 125, 145, 160, 240, 247, 22, 207, 87, 227, 184, 170, 246, 48, 64, 118, 58 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AesKey");
        }
    }
}
