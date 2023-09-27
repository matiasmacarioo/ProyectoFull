using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Migrations
{
    public partial class MigracionAsignaturas2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Asignaturas",
                columns: table => new
                {
                    AsignaturaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarreraID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignaturas", x => x.AsignaturaID);
                    table.ForeignKey(
                        name: "FK_Asignaturas_Carreras_CarreraID",
                        column: x => x.CarreraID,
                        principalTable: "Carreras",
                        principalColumn: "CarreraID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asignaturas_CarreraID",
                table: "Asignaturas",
                column: "CarreraID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asignaturas");
        }
    }
}
