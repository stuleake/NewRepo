using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Data.FormEngine.Migrations
{
    public partial class SectionFieldMapping_FieldNo_String_to_Int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FieldNo",
                schema: "forms",
                table: "SectionFieldMappings",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FieldNo",
                schema: "forms",
                table: "SectionFieldMappings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
