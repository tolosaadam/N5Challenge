using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N5Challenge.Api.Infraestructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class actualizodataseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PermissionTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Permiso 1");

            migrationBuilder.UpdateData(
                table: "PermissionTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Permiso 2");

            migrationBuilder.InsertData(
                table: "PermissionTypes",
                columns: new[] { "Id", "Description" },
                values: new object[] { 3, "Permiso 3" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EmployeeFirstName", "EmployeeLastName" },
                values: new object[] { "Tolosa", "Adam" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "PermissionTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "unknown 1");

            migrationBuilder.UpdateData(
                table: "PermissionTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "unknown 2");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EmployeeFirstName", "EmployeeLastName" },
                values: new object[] { "Berenice", "Spinelli" });
        }
    }
}
