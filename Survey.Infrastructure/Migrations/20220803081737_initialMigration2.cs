using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Migrations
{
    public partial class initialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d327389-6835-47ad-a7ef-fd8f5ae55899");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b90eaac-c59a-4ae5-8b81-bca59e221ee0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f310e9a0-3fb0-4253-89ba-5c4a9699385f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8dd7e26e-8d83-42b9-b9e9-b1e3353d4312", "eb23cd00-099f-4884-b457-fcbfa6c127fa", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9a2582b1-c03a-493d-80af-d8d818242d31", "b47e23ed-ca86-4dd6-bea8-342314f4594c", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f3ea4323-4310-4748-b5ce-15d6cd6a8132", 0, "4f5a75cb-5b4f-4ccd-8d78-f8ca0f77191d", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "08ac5e4f-cd8c-44f1-a06b-d563aefec85e", false, "edin.muhic@forsta.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8dd7e26e-8d83-42b9-b9e9-b1e3353d4312");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a2582b1-c03a-493d-80af-d8d818242d31");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f3ea4323-4310-4748-b5ce-15d6cd6a8132");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4d327389-6835-47ad-a7ef-fd8f5ae55899", "ef05a6a5-1434-4a7a-9ff3-34e639ae5610", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7b90eaac-c59a-4ae5-8b81-bca59e221ee0", "395e12d0-609f-4e54-aceb-5434543287f6", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f310e9a0-3fb0-4253-89ba-5c4a9699385f", 0, "d7a95497-c93f-4146-aa9a-ca7d75c4416e", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "1a0be691-1d74-4413-9b32-452879b3051e", false, "edin.muhic@forsta.com" });
        }
    }
}
