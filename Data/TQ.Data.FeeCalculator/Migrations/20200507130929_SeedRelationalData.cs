using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Data.FeeCalculator.Migrations
{
    public partial class SeedRelationalData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "DefPackages",
                keyColumn: "DefPackageId",
                keyValue: new Guid("1209b615-dd9c-4565-a5ff-5c9385d78179"),
                column: "CreatedDate",
                value: new DateTime(2020, 5, 7, 13, 9, 29, 144, DateTimeKind.Utc).AddTicks(9085));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "DefPackages",
                keyColumn: "DefPackageId",
                keyValue: new Guid("97549ee2-4da5-4439-ae02-3190d233d400"),
                column: "CreatedDate",
                value: new DateTime(2020, 5, 7, 13, 9, 29, 144, DateTimeKind.Utc).AddTicks(7303));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Qsr",
                keyColumn: "QsrId",
                keyValue: new Guid("7fd1318f-0694-4abc-826f-3fef22f0bc3c"),
                column: "CreateDate",
                value: new DateTime(2020, 5, 7, 13, 9, 29, 149, DateTimeKind.Utc).AddTicks(2415));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Qsr",
                keyColumn: "QsrId",
                keyValue: new Guid("f59d9eea-e696-4e75-adf5-4d2254187e58"),
                column: "CreateDate",
                value: new DateTime(2020, 5, 7, 13, 9, 29, 149, DateTimeKind.Utc).AddTicks(1349));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "RuleDefs",
                keyColumn: "RuleDefId",
                keyValue: new Guid("77b2a544-0c47-4e92-a82a-e0c51504db0f"),
                columns: new[] { "CreatedDate", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2020, 5, 7, 13, 9, 29, 150, DateTimeKind.Utc).AddTicks(9031), new DateTime(2020, 5, 14, 13, 9, 29, 150, DateTimeKind.Utc).AddTicks(8490), new DateTime(2020, 5, 7, 13, 9, 29, 150, DateTimeKind.Utc).AddTicks(8061) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "RuleDefs",
                keyColumn: "RuleDefId",
                keyValue: new Guid("dffd4f17-6643-41a8-b444-236f701eae79"),
                columns: new[] { "CreatedDate", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2020, 5, 7, 13, 9, 29, 151, DateTimeKind.Utc).AddTicks(62), new DateTime(2020, 5, 16, 13, 9, 29, 151, DateTimeKind.Utc).AddTicks(52), new DateTime(2020, 5, 7, 13, 9, 29, 151, DateTimeKind.Utc).AddTicks(7) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "TestSetHeaders",
                keyColumn: "TestSetHeaderId",
                keyValue: new Guid("69c08242-2666-4dcd-a2f6-6874ac875aa8"),
                column: "CreatedDate",
                value: new DateTime(2020, 5, 7, 13, 9, 29, 154, DateTimeKind.Utc).AddTicks(6183));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "TestSetHeaders",
                keyColumn: "TestSetHeaderId",
                keyValue: new Guid("cdd6da23-380d-4a70-8b34-c8a0d61417c3"),
                column: "CreatedDate",
                value: new DateTime(2020, 5, 7, 13, 9, 29, 154, DateTimeKind.Utc).AddTicks(5232));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "DefPackages",
                keyColumn: "DefPackageId",
                keyValue: new Guid("1209b615-dd9c-4565-a5ff-5c9385d78179"),
                column: "CreatedDate",
                value: new DateTime(2020, 5, 7, 12, 25, 45, 47, DateTimeKind.Utc).AddTicks(8183));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "DefPackages",
                keyColumn: "DefPackageId",
                keyValue: new Guid("97549ee2-4da5-4439-ae02-3190d233d400"),
                column: "CreatedDate",
                value: new DateTime(2020, 5, 7, 12, 25, 45, 47, DateTimeKind.Utc).AddTicks(6269));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Qsr",
                keyColumn: "QsrId",
                keyValue: new Guid("7fd1318f-0694-4abc-826f-3fef22f0bc3c"),
                column: "CreateDate",
                value: new DateTime(2020, 5, 7, 12, 25, 45, 51, DateTimeKind.Utc).AddTicks(9190));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Qsr",
                keyColumn: "QsrId",
                keyValue: new Guid("f59d9eea-e696-4e75-adf5-4d2254187e58"),
                column: "CreateDate",
                value: new DateTime(2020, 5, 7, 12, 25, 45, 51, DateTimeKind.Utc).AddTicks(8163));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "RuleDefs",
                keyColumn: "RuleDefId",
                keyValue: new Guid("77b2a544-0c47-4e92-a82a-e0c51504db0f"),
                columns: new[] { "CreatedDate", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2020, 5, 7, 12, 25, 45, 53, DateTimeKind.Utc).AddTicks(6575), new DateTime(2020, 5, 14, 12, 25, 45, 53, DateTimeKind.Utc).AddTicks(6043), new DateTime(2020, 5, 7, 12, 25, 45, 53, DateTimeKind.Utc).AddTicks(5587) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "RuleDefs",
                keyColumn: "RuleDefId",
                keyValue: new Guid("dffd4f17-6643-41a8-b444-236f701eae79"),
                columns: new[] { "CreatedDate", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2020, 5, 7, 12, 25, 45, 53, DateTimeKind.Utc).AddTicks(7584), new DateTime(2020, 5, 16, 12, 25, 45, 53, DateTimeKind.Utc).AddTicks(7574), new DateTime(2020, 5, 7, 12, 25, 45, 53, DateTimeKind.Utc).AddTicks(7541) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "TestSetHeaders",
                keyColumn: "TestSetHeaderId",
                keyValue: new Guid("69c08242-2666-4dcd-a2f6-6874ac875aa8"),
                column: "CreatedDate",
                value: new DateTime(2020, 5, 7, 12, 25, 45, 57, DateTimeKind.Utc).AddTicks(3490));

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "TestSetHeaders",
                keyColumn: "TestSetHeaderId",
                keyValue: new Guid("cdd6da23-380d-4a70-8b34-c8a0d61417c3"),
                column: "CreatedDate",
                value: new DateTime(2020, 5, 7, 12, 25, 45, 57, DateTimeKind.Utc).AddTicks(2385));
        }
    }
}
