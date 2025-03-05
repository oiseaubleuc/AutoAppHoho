using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoAppHoho.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e7745171-de0b-40a1-bd71-b138d930a703", "ba77e7e6-a17e-45a0-8c77-3def9aea4517" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c61a5e86-cad0-4155-adce-6c013f5bab33", "c06fc0b0-0237-401b-ae3a-873d1bd88eb9" });
        }
    }
}
