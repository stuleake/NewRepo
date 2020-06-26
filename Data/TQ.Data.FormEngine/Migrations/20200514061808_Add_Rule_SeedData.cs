using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Data.FormEngine.Migrations
{
    public partial class Add_Rule_SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "forms",
                table: "Rules",
                columns: new[] { "RuleId", "Rules" },
                values: new object[] { 1, "Any" });

            migrationBuilder.InsertData(
                schema: "forms",
                table: "Rules",
                columns: new[] { "RuleId", "Rules" },
                values: new object[] { 2, "All" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "forms",
                table: "Rules",
                keyColumn: "RuleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "Rules",
                keyColumn: "RuleId",
                keyValue: 2);
        }
    }
}
