using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Migrations
{
    public partial class TestingNewCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34b6b269-e9fd-41e0-a290-a96bb8f61a24");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d09f797e-a461-4489-a1df-f43342104573");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c7c094e8-4384-4db4-bf1f-023712c981db");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "85096ac8-cad9-4758-b2bb-265dd56b4fad", "022a7ff1-7451-4966-820b-06790a870275", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "93c4e845-0098-4fe5-a69d-0a9607a30e3b", "7234d9e6-1349-43b8-b595-165ccdc5e434", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "50fb8acc-d45e-4585-a62f-6387e3ff16c3", 0, "cadb3c5d-b769-40df-85d0-e9f82c5c5fe6", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "d280d766-7c6e-407d-bd5a-ee9048493bd2", false, "edin.muhic@forsta.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85096ac8-cad9-4758-b2bb-265dd56b4fad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93c4e845-0098-4fe5-a69d-0a9607a30e3b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "50fb8acc-d45e-4585-a62f-6387e3ff16c3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "34b6b269-e9fd-41e0-a290-a96bb8f61a24", "c8cb43c7-ae81-4299-8563-fbfcbf296475", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d09f797e-a461-4489-a1df-f43342104573", "6e7919d4-ee12-4328-8b3a-6b01c351de01", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c7c094e8-4384-4db4-bf1f-023712c981db", 0, "696aa737-66d3-4ac4-a747-1eb4b5a0898e", "edin.muhic@forsta.com", false, "Edin", "Muhic", false, null, "EDIN.MUHIC@FORSTA.COM", "EDIN.MUHIC@FORSTA.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "21653d9b-ea71-4482-90d7-3fe486f10a6f", false, "edin.muhic@forsta.com" });
        }
    }
}
