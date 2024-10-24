using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerIpsumOriginal.Migrations
{
    /// <inheritdoc />
    public partial class AlterCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Conteudos_ConteudoModelId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_ConteudoModelId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "ConteudoModelId",
                table: "Categorias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConteudoModelId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_ConteudoModelId",
                table: "Categorias",
                column: "ConteudoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Conteudos_ConteudoModelId",
                table: "Categorias",
                column: "ConteudoModelId",
                principalTable: "Conteudos",
                principalColumn: "Id");
        }
    }
}
