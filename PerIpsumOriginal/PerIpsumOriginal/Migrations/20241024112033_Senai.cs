using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerIpsumOriginal.Migrations
{
    /// <inheritdoc />
    public partial class Senai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConteudoAprovarCategorias_Categorias_CategoriasId",
                table: "ConteudoAprovarCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ConteudoCategorias_Categorias_CategoriasId",
                table: "ConteudoCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ConteudoRascunhoCategorias_Categorias_CategoriasId",
                table: "ConteudoRascunhoCategorias");

            migrationBuilder.DropTable(
                name: "Calendarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConteudoRascunhoCategorias",
                table: "ConteudoRascunhoCategorias");

            migrationBuilder.DropIndex(
                name: "IX_ConteudoRascunhoCategorias_ConteudoRascunhoId",
                table: "ConteudoRascunhoCategorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConteudoCategorias",
                table: "ConteudoCategorias");

            migrationBuilder.DropIndex(
                name: "IX_ConteudoCategorias_ConteudoId",
                table: "ConteudoCategorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConteudoAprovarCategorias",
                table: "ConteudoAprovarCategorias");

            migrationBuilder.DropIndex(
                name: "IX_ConteudoAprovarCategorias_ConteudoAprovarId",
                table: "ConteudoAprovarCategorias");

            migrationBuilder.RenameColumn(
                name: "CategoriasId",
                table: "ConteudoRascunhoCategorias",
                newName: "CategoriaId");

            migrationBuilder.RenameColumn(
                name: "CategoriasId",
                table: "ConteudoCategorias",
                newName: "CategoriaId");

            migrationBuilder.RenameColumn(
                name: "CategoriasId",
                table: "ConteudoAprovarCategorias",
                newName: "CategoriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConteudoRascunhoCategorias",
                table: "ConteudoRascunhoCategorias",
                columns: new[] { "ConteudoRascunhoId", "CategoriaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConteudoCategorias",
                table: "ConteudoCategorias",
                columns: new[] { "ConteudoId", "CategoriaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConteudoAprovarCategorias",
                table: "ConteudoAprovarCategorias",
                columns: new[] { "ConteudoAprovarId", "CategoriaId" });

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
                name: "IX_ConteudoRascunhoCategorias_CategoriaId",
                table: "ConteudoRascunhoCategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoCategorias_CategoriaId",
                table: "ConteudoCategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoAprovarCategorias_CategoriaId",
                table: "ConteudoAprovarCategorias",
                column: "CategoriaId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ConteudoAprovarCategorias_Categorias_CategoriaId",
                table: "ConteudoAprovarCategorias",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConteudoCategorias_Categorias_CategoriaId",
                table: "ConteudoCategorias",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConteudoRascunhoCategorias_Categorias_CategoriaId",
                table: "ConteudoRascunhoCategorias",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConteudoAprovarCategorias_Categorias_CategoriaId",
                table: "ConteudoAprovarCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ConteudoCategorias_Categorias_CategoriaId",
                table: "ConteudoCategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ConteudoRascunhoCategorias_Categorias_CategoriaId",
                table: "ConteudoRascunhoCategorias");

            migrationBuilder.DropTable(
                name: "CategoriaModelConteudoAprovarModel");

            migrationBuilder.DropTable(
                name: "CategoriaModelConteudoModel");

            migrationBuilder.DropTable(
                name: "CategoriaModelConteudoRascunhoModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConteudoRascunhoCategorias",
                table: "ConteudoRascunhoCategorias");

            migrationBuilder.DropIndex(
                name: "IX_ConteudoRascunhoCategorias_CategoriaId",
                table: "ConteudoRascunhoCategorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConteudoCategorias",
                table: "ConteudoCategorias");

            migrationBuilder.DropIndex(
                name: "IX_ConteudoCategorias_CategoriaId",
                table: "ConteudoCategorias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConteudoAprovarCategorias",
                table: "ConteudoAprovarCategorias");

            migrationBuilder.DropIndex(
                name: "IX_ConteudoAprovarCategorias_CategoriaId",
                table: "ConteudoAprovarCategorias");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "ConteudoRascunhoCategorias",
                newName: "CategoriasId");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "ConteudoCategorias",
                newName: "CategoriasId");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "ConteudoAprovarCategorias",
                newName: "CategoriasId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConteudoRascunhoCategorias",
                table: "ConteudoRascunhoCategorias",
                columns: new[] { "CategoriasId", "ConteudoRascunhoId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConteudoCategorias",
                table: "ConteudoCategorias",
                columns: new[] { "CategoriasId", "ConteudoId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConteudoAprovarCategorias",
                table: "ConteudoAprovarCategorias",
                columns: new[] { "CategoriasId", "ConteudoAprovarId" });

            migrationBuilder.CreateTable(
                name: "Calendarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calendarios_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoRascunhoCategorias_ConteudoRascunhoId",
                table: "ConteudoRascunhoCategorias",
                column: "ConteudoRascunhoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoCategorias_ConteudoId",
                table: "ConteudoCategorias",
                column: "ConteudoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConteudoAprovarCategorias_ConteudoAprovarId",
                table: "ConteudoAprovarCategorias",
                column: "ConteudoAprovarId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendarios_UsuarioId",
                table: "Calendarios",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConteudoAprovarCategorias_Categorias_CategoriasId",
                table: "ConteudoAprovarCategorias",
                column: "CategoriasId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConteudoCategorias_Categorias_CategoriasId",
                table: "ConteudoCategorias",
                column: "CategoriasId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConteudoRascunhoCategorias_Categorias_CategoriasId",
                table: "ConteudoRascunhoCategorias",
                column: "CategoriasId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
