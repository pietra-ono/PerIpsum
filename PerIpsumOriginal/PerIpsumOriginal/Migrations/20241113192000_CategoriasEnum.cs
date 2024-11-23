using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerIpsumOriginal.Migrations
{
    /// <inheritdoc />
    public partial class CategoriasEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConteudoAprovarCategorias");

            migrationBuilder.DropTable(
                name: "ConteudoCategorias");

            migrationBuilder.DropTable(
                name: "ConteudoRascunhoCategorias");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.AddColumn<string>(
                name: "Categorias",
                table: "Conteudos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Categorias",
                table: "ConteudoRascunho",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Categorias",
                table: "ConteudoAprovar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categorias",
                table: "Conteudos");

            migrationBuilder.DropColumn(
                name: "Categorias",
                table: "ConteudoRascunho");

            migrationBuilder.DropColumn(
                name: "Categorias",
                table: "ConteudoAprovar");

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConteudoAprovarCategorias",
                columns: table => new
                {
                    ConteudoAprovarId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConteudoAprovarCategorias", x => new { x.ConteudoAprovarId, x.CategoriaId });
                    table.ForeignKey(
                        name: "FK_ConteudoAprovarCategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConteudoAprovarCategorias_ConteudoAprovar_ConteudoAprovarId",
                        column: x => x.ConteudoAprovarId,
                        principalTable: "ConteudoAprovar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConteudoCategorias",
                columns: table => new
                {
                    ConteudoId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConteudoCategorias", x => new { x.ConteudoId, x.CategoriaId });
                    table.ForeignKey(
                        name: "FK_ConteudoCategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConteudoCategorias_Conteudos_ConteudoId",
                        column: x => x.ConteudoId,
                        principalTable: "Conteudos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConteudoRascunhoCategorias",
                columns: table => new
                {
                    ConteudoRascunhoId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConteudoRascunhoCategorias", x => new { x.ConteudoRascunhoId, x.CategoriaId });
                    table.ForeignKey(
                        name: "FK_ConteudoRascunhoCategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConteudoRascunhoCategorias_ConteudoRascunho_ConteudoRascunhoId",
                        column: x => x.ConteudoRascunhoId,
                        principalTable: "ConteudoRascunho",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoAprovarCategorias_CategoriaId",
                table: "ConteudoAprovarCategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoCategorias_CategoriaId",
                table: "ConteudoCategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoRascunhoCategorias_CategoriaId",
                table: "ConteudoRascunhoCategorias",
                column: "CategoriaId");
        }
    }
}
