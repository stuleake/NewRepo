using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TQ.Data.FeeCalculator.Migrations
{
    public partial class SeedTypeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("698cd64a-b87a-47cf-915c-f863704179c5"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("835939c7-6281-4872-91cc-cdec8f90b07e"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("bd71f495-4581-402f-9869-4bd2a7c7b5bf"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "OutputOperationTypes",
                keyColumn: "OutputOperationId",
                keyValue: new Guid("6b5fe5fe-bbc0-498b-912b-65c42c2ec9d9"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "OutputOperationTypes",
                keyColumn: "OutputOperationId",
                keyValue: new Guid("ec15a673-3464-4c34-84a7-a49dacf42f0c"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParamDataTypes",
                keyColumn: "ParamDataTypeId",
                keyValue: new Guid("5a7b05e4-890c-44bf-bd3f-43d6b5175611"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParamDataTypes",
                keyColumn: "ParamDataTypeId",
                keyValue: new Guid("b57f6d63-e2c5-4489-98ef-508c3bdab6bf"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParamDataTypes",
                keyColumn: "ParamDataTypeId",
                keyValue: new Guid("ccc73977-4444-4ec3-b387-e49d139d2857"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParamDataTypes",
                keyColumn: "ParamDataTypeId",
                keyValue: new Guid("d2328fe2-9a72-4b2f-8657-98355df45c0a"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParameterTypes",
                keyColumn: "ParameterTypeId",
                keyValue: new Guid("b446b9a6-7260-4cc8-82d9-deaea404236e"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParameterTypes",
                keyColumn: "ParameterTypeId",
                keyValue: new Guid("df8a5758-a81b-453a-9eb9-1119c8be0e19"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "SessionTypes",
                keyColumn: "SessionTypeId",
                keyValue: new Guid("730d5359-c45c-431a-943b-c66475208f51"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "SessionTypes",
                keyColumn: "SessionTypeId",
                keyValue: new Guid("bdc48b95-aae8-475e-96f9-09b7ec4e7c28"));

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Sequence" },
                values: new object[,]
                {
                    { new Guid("6abc716e-7481-4993-84de-da80a385d096"), "FeeRules", 1 },
                    { new Guid("98e61c33-50c3-4540-8be7-e5c28ea5a664"), "ConcessionRules", 2 },
                    { new Guid("a8d9d6a1-263d-4ccb-a328-2547846f3122"), "ServiceChargeRules", 3 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "OutputOperationTypes",
                columns: new[] { "OutputOperationId", "Name" },
                values: new object[,]
                {
                    { new Guid("10380bc9-98dd-49c2-b9ca-b0d316f9707a"), "add" },
                    { new Guid("ccfcb7fc-4705-4cf6-904e-cd2bc382ab9e"), "overwrite" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ParamDataTypes",
                columns: new[] { "ParamDataTypeId", "ParamDataTypeName" },
                values: new object[,]
                {
                    { new Guid("d97c5bd8-a109-49a8-ba5d-c9ee3ebdff44"), "bool" },
                    { new Guid("835187eb-99cf-4077-94d7-369d0fd4ecd7"), "number" },
                    { new Guid("c44ec053-0681-449b-9c1b-b5d86782f645"), "int" },
                    { new Guid("2eba37d4-be44-4fc7-bb92-ed583d044917"), "string" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ParameterTypes",
                columns: new[] { "ParameterTypeId", "Name" },
                values: new object[,]
                {
                    { new Guid("6c6b863f-728f-444c-802c-d1b66d92d7f3"), "in" },
                    { new Guid("7e4f3023-43bb-41dc-9cf5-7f873a04c2fc"), "out" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "SessionTypes",
                columns: new[] { "SessionTypeId", "Name" },
                values: new object[,]
                {
                    { new Guid("1145d244-fea5-45b0-9927-9c21dcd34566"), "TestSet" },
                    { new Guid("024512ec-6b91-4165-bb6a-0cfe9dc0965c"), "QSR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("6abc716e-7481-4993-84de-da80a385d096"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("98e61c33-50c3-4540-8be7-e5c28ea5a664"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("a8d9d6a1-263d-4ccb-a328-2547846f3122"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "OutputOperationTypes",
                keyColumn: "OutputOperationId",
                keyValue: new Guid("10380bc9-98dd-49c2-b9ca-b0d316f9707a"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "OutputOperationTypes",
                keyColumn: "OutputOperationId",
                keyValue: new Guid("ccfcb7fc-4705-4cf6-904e-cd2bc382ab9e"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParamDataTypes",
                keyColumn: "ParamDataTypeId",
                keyValue: new Guid("2eba37d4-be44-4fc7-bb92-ed583d044917"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParamDataTypes",
                keyColumn: "ParamDataTypeId",
                keyValue: new Guid("835187eb-99cf-4077-94d7-369d0fd4ecd7"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParamDataTypes",
                keyColumn: "ParamDataTypeId",
                keyValue: new Guid("c44ec053-0681-449b-9c1b-b5d86782f645"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParamDataTypes",
                keyColumn: "ParamDataTypeId",
                keyValue: new Guid("d97c5bd8-a109-49a8-ba5d-c9ee3ebdff44"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParameterTypes",
                keyColumn: "ParameterTypeId",
                keyValue: new Guid("6c6b863f-728f-444c-802c-d1b66d92d7f3"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ParameterTypes",
                keyColumn: "ParameterTypeId",
                keyValue: new Guid("7e4f3023-43bb-41dc-9cf5-7f873a04c2fc"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "SessionTypes",
                keyColumn: "SessionTypeId",
                keyValue: new Guid("024512ec-6b91-4165-bb6a-0cfe9dc0965c"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "SessionTypes",
                keyColumn: "SessionTypeId",
                keyValue: new Guid("1145d244-fea5-45b0-9927-9c21dcd34566"));

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Sequence" },
                values: new object[,]
                {
                    { new Guid("bd71f495-4581-402f-9869-4bd2a7c7b5bf"), "FeeRules", 1 },
                    { new Guid("835939c7-6281-4872-91cc-cdec8f90b07e"), "ConcessionRules", 2 },
                    { new Guid("698cd64a-b87a-47cf-915c-f863704179c5"), "ServiceChargeRules", 3 }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "OutputOperationTypes",
                columns: new[] { "OutputOperationId", "Name" },
                values: new object[,]
                {
                    { new Guid("6b5fe5fe-bbc0-498b-912b-65c42c2ec9d9"), "add" },
                    { new Guid("ec15a673-3464-4c34-84a7-a49dacf42f0c"), "overwrite" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ParamDataTypes",
                columns: new[] { "ParamDataTypeId", "ParamDataTypeName" },
                values: new object[,]
                {
                    { new Guid("ccc73977-4444-4ec3-b387-e49d139d2857"), "bool" },
                    { new Guid("5a7b05e4-890c-44bf-bd3f-43d6b5175611"), "number" },
                    { new Guid("d2328fe2-9a72-4b2f-8657-98355df45c0a"), "int" },
                    { new Guid("b57f6d63-e2c5-4489-98ef-508c3bdab6bf"), "string" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ParameterTypes",
                columns: new[] { "ParameterTypeId", "Name" },
                values: new object[,]
                {
                    { new Guid("b446b9a6-7260-4cc8-82d9-deaea404236e"), "in" },
                    { new Guid("df8a5758-a81b-453a-9eb9-1119c8be0e19"), "out" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "SessionTypes",
                columns: new[] { "SessionTypeId", "Name" },
                values: new object[,]
                {
                    { new Guid("730d5359-c45c-431a-943b-c66475208f51"), "TestSet" },
                    { new Guid("bdc48b95-aae8-475e-96f9-09b7ec4e7c28"), "QSR" }
                });
        }
    }
}
