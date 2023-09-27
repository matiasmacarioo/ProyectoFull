using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Migrations
{
    public partial class Mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumno_Carrera_CarreraID",
                table: "Alumno");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carrera",
                table: "Carrera");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alumno",
                table: "Alumno");

            migrationBuilder.RenameTable(
                name: "Carrera",
                newName: "Carreras");

            migrationBuilder.RenameTable(
                name: "Alumno",
                newName: "Alumnos");

            migrationBuilder.RenameIndex(
                name: "IX_Alumno_CarreraID",
                table: "Alumnos",
                newName: "IX_Alumnos_CarreraID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carreras",
                table: "Carreras",
                column: "CarreraID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alumnos",
                table: "Alumnos",
                column: "AlumnoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnos_Carreras_CarreraID",
                table: "Alumnos",
                column: "CarreraID",
                principalTable: "Carreras",
                principalColumn: "CarreraID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnos_Carreras_CarreraID",
                table: "Alumnos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carreras",
                table: "Carreras");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Alumnos",
                table: "Alumnos");

            migrationBuilder.RenameTable(
                name: "Carreras",
                newName: "Carrera");

            migrationBuilder.RenameTable(
                name: "Alumnos",
                newName: "Alumno");

            migrationBuilder.RenameIndex(
                name: "IX_Alumnos_CarreraID",
                table: "Alumno",
                newName: "IX_Alumno_CarreraID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carrera",
                table: "Carrera",
                column: "CarreraID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Alumno",
                table: "Alumno",
                column: "AlumnoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumno_Carrera_CarreraID",
                table: "Alumno",
                column: "CarreraID",
                principalTable: "Carrera",
                principalColumn: "CarreraID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
