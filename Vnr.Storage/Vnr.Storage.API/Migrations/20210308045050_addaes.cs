using Microsoft.EntityFrameworkCore.Migrations;

namespace Vnr.Storage.API.Migrations
{
    public partial class addaes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AesKey",
                table: "AesKey");

            migrationBuilder.RenameTable(
                name: "AesKey",
                newName: "AesKeys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AesKeys",
                table: "AesKeys",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AesKeys",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "IV", "Key" },
                values: new object[] { new byte[] { 60, 194, 198, 149, 105, 243, 198, 134, 105, 137, 179, 13, 229, 165, 139, 52 }, new byte[] { 28, 117, 42, 194, 1, 117, 71, 68, 139, 169, 0, 154, 31, 28, 204, 29, 82, 77, 122, 1, 117, 73, 234, 115, 68, 255, 108, 46, 85, 111, 150, 165 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AesKeys",
                table: "AesKeys");

            migrationBuilder.RenameTable(
                name: "AesKeys",
                newName: "AesKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AesKey",
                table: "AesKey",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AesKey",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "IV", "Key" },
                values: new object[] { new byte[] { 47, 229, 196, 4, 91, 184, 189, 79, 12, 248, 225, 192, 253, 28, 118, 211 }, new byte[] { 231, 188, 153, 49, 147, 194, 81, 240, 141, 247, 144, 249, 104, 36, 170, 136, 125, 145, 160, 240, 247, 22, 207, 87, 227, 184, 170, 246, 48, 64, 118, 58 } });
        }
    }
}