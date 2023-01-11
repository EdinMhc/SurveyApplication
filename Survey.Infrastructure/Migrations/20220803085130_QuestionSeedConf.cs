using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Migrations
{
    public partial class QuestionSeedConf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2407d7f8-5e75-4e48-ad05-75b1ada4ac97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc3adb24-67fb-4893-95c1-b9a1687c3eda");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "46668a46-7d86-4a33-8dea-2567a6e4312a");

            migrationBuilder.RenameColumn(
                name: "QuestionID",
                table: "Question",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Question",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "Question",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Question",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "QuestionType",
                table: "Question",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "Question",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Question",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2407d7f8-5e75-4e48-ad05-75b1ada4ac97", "94b173f0-dd8d-4751-9ee6-e391e3b669ce", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fc3adb24-67fb-4893-95c1-b9a1687c3eda", "046c5cd4-d5e2-450a-ac53-a70643cfec4a", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "46668a46-7d86-4a33-8dea-2567a6e4312a", 0, "3ddf76fc-0ed8-431d-9620-50c1d401d6b0", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "955484ff-6ce2-4a6e-a360-ad675f2fe786", false, "edin.muhic@forsta.com" });
        }
    }
}
