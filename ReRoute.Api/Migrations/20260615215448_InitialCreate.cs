using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReRoute.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESG_PARTICIPANTE",
                columns: table => new
                {
                    PARTICIPANTID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    TIPOPERFIL = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    CREATEDAT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_PARTICIPANTE", x => x.PARTICIPANTID);
                });

            migrationBuilder.CreateTable(
                name: "ESG_DOACAO",
                columns: table => new
                {
                    DONATIONID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PARTICIPANTID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NOMEITEM = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    TAMANHO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    LOCALRETIRADA = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    CREATEDAT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_DOACAO", x => x.DONATIONID);
                    table.ForeignKey(
                        name: "FK_ESG_DOACAO_ESG_PARTICIPANTE_PARTICIPANTID",
                        column: x => x.PARTICIPANTID,
                        principalTable: "ESG_PARTICIPANTE",
                        principalColumn: "PARTICIPANTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ESG_REGISTRO",
                columns: table => new
                {
                    REGISTROID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DONATIONID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    RESIDUOEVITADO = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: false),
                    CO2EVITADO = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: false),
                    DATAREGISTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_REGISTRO", x => x.REGISTROID);
                    table.ForeignKey(
                        name: "FK_ESG_REGISTRO_ESG_DOACAO_DONATIONID",
                        column: x => x.DONATIONID,
                        principalTable: "ESG_DOACAO",
                        principalColumn: "DONATIONID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ESG_SOLICITACAO",
                columns: table => new
                {
                    SOLICITACAOID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DONATIONID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PARTICIPANTID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DATASOLICITACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_SOLICITACAO", x => x.SOLICITACAOID);
                    table.ForeignKey(
                        name: "FK_ESG_SOLICITACAO_ESG_DOACAO_DONATIONID",
                        column: x => x.DONATIONID,
                        principalTable: "ESG_DOACAO",
                        principalColumn: "DONATIONID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ESG_SOLICITACAO_ESG_PARTICIPANTE_PARTICIPANTID",
                        column: x => x.PARTICIPANTID,
                        principalTable: "ESG_PARTICIPANTE",
                        principalColumn: "PARTICIPANTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ESG_ENTREGA",
                columns: table => new
                {
                    ENTREGAID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SOLICITACAOID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PARTICIPANTID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DATAACEITE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATAENTREGA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    STATUS = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_ENTREGA", x => x.ENTREGAID);
                    table.ForeignKey(
                        name: "FK_ESG_ENTREGA_ESG_PARTICIPANTE_PARTICIPANTID",
                        column: x => x.PARTICIPANTID,
                        principalTable: "ESG_PARTICIPANTE",
                        principalColumn: "PARTICIPANTID");
                    table.ForeignKey(
                        name: "FK_ESG_ENTREGA_ESG_SOLICITACAO_SOLICITACAOID",
                        column: x => x.SOLICITACAOID,
                        principalTable: "ESG_SOLICITACAO",
                        principalColumn: "SOLICITACAOID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ESG_DOACAO_PARTICIPANTID",
                table: "ESG_DOACAO",
                column: "PARTICIPANTID");

            migrationBuilder.CreateIndex(
                name: "IX_ESG_ENTREGA_PARTICIPANTID",
                table: "ESG_ENTREGA",
                column: "PARTICIPANTID");

            migrationBuilder.CreateIndex(
                name: "IX_ESG_ENTREGA_SOLICITACAOID",
                table: "ESG_ENTREGA",
                column: "SOLICITACAOID");

            migrationBuilder.CreateIndex(
                name: "ESG_REGISTRO_IDX",
                table: "ESG_REGISTRO",
                column: "DONATIONID");

            migrationBuilder.CreateIndex(
                name: "IX_ESG_SOLICITACAO_DONATIONID",
                table: "ESG_SOLICITACAO",
                column: "DONATIONID");

            migrationBuilder.CreateIndex(
                name: "IX_ESG_SOLICITACAO_PARTICIPANTID",
                table: "ESG_SOLICITACAO",
                column: "PARTICIPANTID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESG_ENTREGA");

            migrationBuilder.DropTable(
                name: "ESG_REGISTRO");

            migrationBuilder.DropTable(
                name: "ESG_SOLICITACAO");

            migrationBuilder.DropTable(
                name: "ESG_DOACAO");

            migrationBuilder.DropTable(
                name: "ESG_PARTICIPANTE");
        }
    }
}
