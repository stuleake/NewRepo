using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Data.FormEngine.Migrations
{
    public partial class _2059_DisplaysSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "forms",
                table: "Displays",
                columns: new[] { "DisplayId", "Displays" },
                values: new object[] { 1, "Hide" });

            migrationBuilder.InsertData(
                schema: "forms",
                table: "Displays",
                columns: new[] { "DisplayId", "Displays" },
                values: new object[] { 2, "Optional" });

            migrationBuilder.InsertData(
                schema: "forms",
                table: "Displays",
                columns: new[] { "DisplayId", "Displays" },
                values: new object[] { 3, "Required" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "forms",
                table: "Displays",
                keyColumn: "DisplayId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "Displays",
                keyColumn: "DisplayId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "Displays",
                keyColumn: "DisplayId",
                keyValue: 3);
        }
    }
}
