using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetoAeC.Migrations
{
    /// <inheritdoc />
    public partial class CreateEnderecos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    cep = table.Column<string>(type: "TEXT", maxLength: 9, nullable: false),
                    logradouro = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    numero = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    complemento = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    bairro = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    cidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    uf = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    ibge = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enderecos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_UsuarioId",
                table: "Enderecos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enderecos");
        }
    }
}
