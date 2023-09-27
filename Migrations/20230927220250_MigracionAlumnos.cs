using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Migrations
{
    public partial class MigracionAlumnos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Alumnos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Documento",
                table: "Alumnos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Alumnos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "Documento",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Alumnos");
        }
    }
}
