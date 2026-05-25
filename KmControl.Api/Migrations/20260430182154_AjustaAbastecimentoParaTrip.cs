using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KmControl.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjustaAbastecimentoParaTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KmAtual",
                table: "Abastecimentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "KmAtual",
                table: "Abastecimentos",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
