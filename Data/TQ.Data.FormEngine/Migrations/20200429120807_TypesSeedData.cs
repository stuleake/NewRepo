using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace TQ.Data.FormEngine.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class TypesSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "forms",
                table: "AnswerTypes",
                columns: new[] { "AnswerTypeId", "AnswerTypes" },
                values: new object[,]
                {
                    { 1, "Range" },
                    { 2, "Length" },
                    { 3, "regex" },
                    { 4, "RegexBE" },
                    { 5, "Multiple" },
                    { 6, "Value" },
                    { 7, "API" },
                    { 8, "Date" },
                    { 9, "copyFrom" }
                });

            migrationBuilder.InsertData(
                schema: "forms",
                table: "FieldTypes",
                columns: new[] { "FieldTypeId", "FieldTypes" },
                values: new object[,]
                {
                    { 13, "CHECKBOX" },
                    { 12, "Notification" },
                    { 11, "NUMBERSELECTOR" },
                    { 10, "TEXT" },
                    { 9, "Aggregation" },
                    { 8, "ActionTable" },
                    { 6, "ActionInput" },
                    { 5, "BUTTON" },
                    { 4, "DROPDOWN" },
                    { 3, "DATE" },
                    { 2, "DROPDOWN" },
                    { 1, "NUMBER" },
                    { 7, "ActionAddress" }
                });

            migrationBuilder.InsertData(
                schema: "forms",
                table: "SectionTypes",
                columns: new[] { "SectionTypeId", "SectionTypes" },
                values: new object[,]
                {
                    { 1, "Main-Fields" },
                    { 2, "Main-Table" },
                    { 3, "Sub-Fields" },
                    { 4, "Sub-Table" }
                });

            migrationBuilder.InsertData(
                schema: "forms",
                table: "Status",
                columns: new[] { "StatusId", "Status" },
                values: new object[,]
                {
                    { 2, "Active" },
                    { 1, "Draft" },
                    { 3, "Legacy" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "AnswerTypes",
                keyColumn: "AnswerTypeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "FieldTypes",
                keyColumn: "FieldTypeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "SectionTypes",
                keyColumn: "SectionTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "SectionTypes",
                keyColumn: "SectionTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "SectionTypes",
                keyColumn: "SectionTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "SectionTypes",
                keyColumn: "SectionTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "Status",
                keyColumn: "StatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "Status",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "forms",
                table: "Status",
                keyColumn: "StatusId",
                keyValue: 3);
        }
    }
}
