using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N5Challenge.Api.Infraestructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class SecondUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_PermissionTypes_TypeId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_TypeId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Permissions");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionTypeId",
                table: "Permissions",
                column: "PermissionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_PermissionTypes_PermissionTypeId",
                table: "Permissions",
                column: "PermissionTypeId",
                principalTable: "PermissionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_PermissionTypes_PermissionTypeId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_PermissionTypeId",
                table: "Permissions");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "TypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "TypeId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_TypeId",
                table: "Permissions",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_PermissionTypes_TypeId",
                table: "Permissions",
                column: "TypeId",
                principalTable: "PermissionTypes",
                principalColumn: "Id");
        }
    }
}
