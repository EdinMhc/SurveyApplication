using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Infrastructure.Migrations
{
    public partial class EasyUseUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0124983-f40a-4577-a3fc-e9764b2f3417");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cfbb4447-2bcd-4c17-820f-ed3a411ffadc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8d646793-afcb-4056-b48a-9adc44381689");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bffed1fb-fbb0-407d-bce2-09d0567294c9", "9fd6390a-515d-4191-92c1-82f1fa626d70", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c48a4f12-a237-4019-9058-87db861f196b", "1244d6fb-2955-4287-8ac9-8f394e24d2d9", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "52e94785-070e-432a-829d-fbf449d2da2b", 0, "d828b219-1797-4abb-8eab-59003e0b4033", "A@A", false, "Edin", "Muhic", false, null, "A@A", "A@A", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "b698fb85-89da-4cca-9f93-d7aca8b7a267", false, "a@a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bffed1fb-fbb0-407d-bce2-09d0567294c9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c48a4f12-a237-4019-9058-87db861f196b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "52e94785-070e-432a-829d-fbf449d2da2b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a0124983-f40a-4577-a3fc-e9764b2f3417", "57c9e353-dc83-406a-981f-9f9c5f175396", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cfbb4447-2bcd-4c17-820f-ed3a411ffadc", "332d4db0-d72a-4a62-89f2-25380e7a9743", "SuperAdmin", "SuperAdmin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8d646793-afcb-4056-b48a-9adc44381689", 0, "04400168-714f-4661-b9d2-64273f03b569", "edinmuhic00@gmail.com", false, "Edin", "Muhic", false, null, "EDINMUHIC00@GMAIL.COM", "EDINMUHIC00@GMAIL.COM", "AQAAAAEAACcQAAAAEMF/jR4CGcZfNBTxLIe5QyaadJ5RFYRfZSh1I/1gfRjjTF9UPjhxDDa3+07E+tGzhQ==", null, false, "9497e336-c916-47b6-a7f6-63c0f10d81f1", false, "edinmuhic00@gmail.com" });
        }
    }
}
