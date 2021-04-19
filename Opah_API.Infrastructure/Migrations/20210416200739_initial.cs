using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Opah_API.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_CIDADE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "varchar(100)", nullable: false),
                    ESTADO = table.Column<string>(type: "char(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CIDADE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_CLIENTE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "varchar(150)", nullable: false),
                    RG = table.Column<string>(type: "varchar(20)", nullable: false),
                    CPF = table.Column<string>(type: "varchar(20)", nullable: false),
                    DATA_NASCIMENTO = table.Column<DateTime>(type: "date", nullable: false),
                    TELEFONE = table.Column<string>(type: "varchar(20)", nullable: false),
                    EMAIL_ = table.Column<string>(type: "varchar(150)", nullable: false),
                    COD_EMPRESA = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CLIENTE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_ENDERECO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RUA = table.Column<string>(type: "varchar(255)", nullable: false),
                    BAIRRO = table.Column<string>(type: "varchar(50)", nullable: false),
                    NUMERO = table.Column<string>(type: "varchar(50)", nullable: false),
                    COMPLEMENTO = table.Column<string>(type: "varchar(100)", nullable: false),
                    CEP = table.Column<string>(type: "varchar(10)", nullable: false),
                    TIPO_ENDERECO = table.Column<int>(type: "int", nullable: false),
                    TB_CIDADE_ID = table.Column<int>(type: "int", nullable: false),
                    TB_CLIENTE_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ENDERECO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ENDERECO_TB_CIDADE_TB_CIDADE_ID",
                        column: x => x.TB_CIDADE_ID,
                        principalTable: "TB_CIDADE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_ENDERECO_TB_CLIENTE_TB_CLIENTE_ID",
                        column: x => x.TB_CLIENTE_ID,
                        principalTable: "TB_CLIENTE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ENDERECO_TB_CIDADE_ID",
                table: "TB_ENDERECO",
                column: "TB_CIDADE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ENDERECO_TB_CLIENTE_ID",
                table: "TB_ENDERECO",
                column: "TB_CLIENTE_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ENDERECO");

            migrationBuilder.DropTable(
                name: "TB_CIDADE");

            migrationBuilder.DropTable(
                name: "TB_CLIENTE");
        }
    }
}
