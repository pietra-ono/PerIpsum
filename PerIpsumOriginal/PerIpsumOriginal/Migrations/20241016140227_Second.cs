using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerIpsumOriginal.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConteudoAprovarCategorias");

            migrationBuilder.DropTable(
                name: "ConteudoCategorias");

            migrationBuilder.AddColumn<int>(
                name: "ConteudoAprovarModelId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConteudoModelId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_ConteudoAprovarModelId",
                table: "Categorias",
                column: "ConteudoAprovarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_ConteudoModelId",
                table: "Categorias",
                column: "ConteudoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_ConteudoAprovar_ConteudoAprovarModelId",
                table: "Categorias",
                column: "ConteudoAprovarModelId",
                principalTable: "ConteudoAprovar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Conteudos_ConteudoModelId",
                table: "Categorias",
                column: "ConteudoModelId",
                principalTable: "Conteudos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_ConteudoAprovar_ConteudoAprovarModelId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Conteudos_ConteudoModelId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_ConteudoAprovarModelId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_ConteudoModelId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "ConteudoAprovarModelId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "ConteudoModelId",
                table: "Categorias");

            migrationBuilder.CreateTable(
                name: "ConteudoAprovarCategorias",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    ConteudoAprovarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConteudoAprovarCategorias", x => new { x.CategoriasId, x.ConteudoAprovarId });
                    table.ForeignKey(
                        name: "FK_ConteudoAprovarCategorias_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
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
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    ConteudoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConteudoCategorias", x => new { x.CategoriasId, x.ConteudoId });
                    table.ForeignKey(
                        name: "FK_ConteudoCategorias_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
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

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoAprovarCategorias_ConteudoAprovarId",
                table: "ConteudoAprovarCategorias",
                column: "ConteudoAprovarId");

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoCategorias_ConteudoId",
                table: "ConteudoCategorias",
                column: "ConteudoId");
        }
    }
}
