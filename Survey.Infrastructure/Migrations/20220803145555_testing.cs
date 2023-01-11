using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Migrations
{
    public partial class testing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7fbfaf0d-073e-4e60-a8e5-1a35e93aca2d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c476cd4b-b5fa-443e-bd1c-cc3502766949");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d0983a25-c84e-43a1-8806-7bd68f446999");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7a921cf5-ffc8-445b-aaa1-964f0d15dfbc", "57006a20-9d08-42a4-bb1b-3f930ef0af7a", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cf04c817-c802-41ee-8c84-0c24c1596735", "a45121ea-0118-4583-8696-79d563073118", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "39ea42fb-2230-4998-b83c-5d5a2e4f70d2", 0, "3b629003-4a2d-4667-b28b-f3228cccb321", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "2d27aecb-2aa6-4649-9073-1740a57ede37", false, "edin.muhic@forsta.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a921cf5-ffc8-445b-aaa1-964f0d15dfbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf04c817-c802-41ee-8c84-0c24c1596735");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "39ea42fb-2230-4998-b83c-5d5a2e4f70d2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7fbfaf0d-073e-4e60-a8e5-1a35e93aca2d", "3b534341-72eb-4430-9301-be96caca8b54", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c476cd4b-b5fa-443e-bd1c-cc3502766949", "cc969ba0-0ae4-42dc-b7ae-a17e3a1ed62f", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d0983a25-c84e-43a1-8806-7bd68f446999", 0, "88870b69-05cb-4116-8412-3f0f4bf8846f", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "1be8ddef-0ee8-47fe-8f21-e04c769e008a", false, "edin.muhic@forsta.com" });
        }
    }
}
