using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Migrations
{
    public partial class ChangedColNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Survey_Company_CompanyID",
                table: "Survey");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16a50368-5178-465c-a8fe-755d999db693");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3ace3e0-3e6d-4ec9-bd5a-8c91bc7826f4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "48d4a076-5b63-4107-a40e-ce118ac9e1aa");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Question",
                newName: "QuestionID");

            migrationBuilder.AlterColumn<string>(
                name: "SurveyName",
                table: "Survey",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Survey",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Survey",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyID",
                table: "Survey",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3ec3bcee-83bf-460d-ad98-a6b321801adc", "b99121d9-c2f9-41d1-b211-bd9a3f542320", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cfc8ea7a-00f8-4777-acd2-f18df499a3f7", "ee2241a5-159c-4c3b-b11f-51a54ac9cded", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "48932ea3-7723-4c53-ae1c-17c5fed7385f", 0, "56c3820f-9b14-4a9a-9818-37ede33c159a", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "594376a0-26db-4ff9-b0ca-c00f5950ae13", false, "edin.muhic@forsta.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_Survey_Company_CompanyID",
                table: "Survey",
                column: "CompanyID",
                principalTable: "Company",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Survey_Company_CompanyID",
                table: "Survey");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ec3bcee-83bf-460d-ad98-a6b321801adc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cfc8ea7a-00f8-4777-acd2-f18df499a3f7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "48932ea3-7723-4c53-ae1c-17c5fed7385f");

            migrationBuilder.RenameColumn(
                name: "QuestionID",
                table: "Question",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "SurveyName",
                table: "Survey",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Survey",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Survey",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyID",
                table: "Survey",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "16a50368-5178-465c-a8fe-755d999db693", "1efe872a-6e41-4589-bb8f-41aa0d26648c", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d3ace3e0-3e6d-4ec9-bd5a-8c91bc7826f4", "04856101-222a-457f-b006-cf50cfe2443f", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "48d4a076-5b63-4107-a40e-ce118ac9e1aa", 0, "8a31a5cd-3769-4459-8b6c-a7b0f956c66e", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "f6fb2f9e-90c9-46bc-a5c4-437cbc27df4e", false, "edin.muhic@forsta.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_Survey_Company_CompanyID",
                table: "Survey",
                column: "CompanyID",
                principalTable: "Company",
                principalColumn: "CompanyID");
        }
    }
}
