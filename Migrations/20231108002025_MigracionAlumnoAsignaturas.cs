using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Migrations
{
    public partial class MigracionAlumnoAsignaturas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlumnoAsignaturas",
                columns: table => new
                {
                    AlumnoAsignaturaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlumnoID = table.Column<int>(type: "int", nullable: false),
                    AsignaturaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumnoAsignaturas", x => x.AlumnoAsignaturaID);
                    table.ForeignKey(
                        name: "FK_AlumnoAsignaturas_Alumnos_AlumnoID",
                        column: x => x.AlumnoID,
                        principalTable: "Alumnos",
                        principalColumn: "AlumnoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlumnoAsignaturas_Asignaturas_AsignaturaID",
                        column: x => x.AsignaturaID,
                        principalTable: "Asignaturas",
                        principalColumn: "AsignaturaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoAsignaturas_AlumnoID",
                table: "AlumnoAsignaturas",
                column: "AlumnoID");

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoAsignaturas_AsignaturaID",
                table: "AlumnoAsignaturas",
                column: "AsignaturaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlumnoAsignaturas");
        }
    }
}
