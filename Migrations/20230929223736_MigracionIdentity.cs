using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Migrations
{
    public partial class MigracionIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityID",
                table: "Profesores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Profesores",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityID",
                table: "Alumnos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Alumnos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profesores_IdentityUserId",
                table: "Profesores",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_IdentityUserId",
                table: "Alumnos",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnos_AspNetUsers_IdentityUserId",
                table: "Alumnos",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profesores_AspNetUsers_IdentityUserId",
                table: "Profesores",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnos_AspNetUsers_IdentityUserId",
                table: "Alumnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Profesores_AspNetUsers_IdentityUserId",
                table: "Profesores");

            migrationBuilder.DropIndex(
                name: "IX_Profesores_IdentityUserId",
                table: "Profesores");

            migrationBuilder.DropIndex(
                name: "IX_Alumnos_IdentityUserId",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "IdentityID",
                table: "Profesores");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Profesores");

            migrationBuilder.DropColumn(
                name: "IdentityID",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Alumnos");
        }
    }
}
