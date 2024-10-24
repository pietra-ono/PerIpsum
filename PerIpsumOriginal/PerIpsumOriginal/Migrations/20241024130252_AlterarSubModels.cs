using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerIpsumOriginal.Migrations
{
    /// <inheritdoc />
    public partial class AlterarSubModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaModelConteudoAprovarModel");

            migrationBuilder.DropTable(
                name: "CategoriaModelConteudoModel");

            migrationBuilder.DropTable(
                name: "CategoriaModelConteudoRascunhoModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriaModelConteudoAprovarModel",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    ConteudoAprovarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaModelConteudoAprovarModel", x => new { x.CategoriasId, x.ConteudoAprovarId });
                    table.ForeignKey(
                        name: "FK_CategoriaModelConteudoAprovarModel_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaModelConteudoAprovarModel_ConteudoAprovar_ConteudoAprovarId",
                        column: x => x.ConteudoAprovarId,
                        principalTable: "ConteudoAprovar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaModelConteudoModel",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    ConteudoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaModelConteudoModel", x => new { x.CategoriasId, x.ConteudoId });
                    table.ForeignKey(
                        name: "FK_CategoriaModelConteudoModel_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaModelConteudoModel_Conteudos_ConteudoId",
                        column: x => x.ConteudoId,
                        principalTable: "Conteudos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaModelConteudoRascunhoModel",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    ConteudoRascunhoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaModelConteudoRascunhoModel", x => new { x.CategoriasId, x.ConteudoRascunhoId });
                    table.ForeignKey(
                        name: "FK_CategoriaModelConteudoRascunhoModel_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaModelConteudoRascunhoModel_ConteudoRascunho_ConteudoRascunhoId",
                        column: x => x.ConteudoRascunhoId,
                        principalTable: "ConteudoRascunho",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaModelConteudoAprovarModel_ConteudoAprovarId",
                table: "CategoriaModelConteudoAprovarModel",
                column: "ConteudoAprovarId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaModelConteudoModel_ConteudoId",
                table: "CategoriaModelConteudoModel",
                column: "ConteudoId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaModelConteudoRascunhoModel_ConteudoRascunhoId",
                table: "CategoriaModelConteudoRascunhoModel",
                column: "ConteudoRascunhoId");
        }
    }
}
