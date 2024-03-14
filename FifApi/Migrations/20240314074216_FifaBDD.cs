using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FifApi.Migrations
{
    public partial class FifaBDD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_e_couleur_clr",
                columns: table => new
                {
                    clr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    clr_nom = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clr", x => x.clr_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_produit_pdt",
                columns: table => new
                {
                    pdt_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pdt_nom = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    pdt_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    pdt_caracteristiques = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pdt", x => x.pdt_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_typeproduit_tpd",
                columns: table => new
                {
                    tpd_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tpd_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tpd_description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    tpd_surtype = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tpd", x => x.tpd_id);
                    table.ForeignKey(
                        name: "fk_tpd_tpd",
                        column: x => x.tpd_surtype,
                        principalTable: "t_e_typeproduit_tpd",
                        principalColumn: "tpd_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_j_couleurproduit_clp",
                columns: table => new
                {
                    clp_produit = table.Column<int>(type: "integer", nullable: false),
                    clp_couleur = table.Column<int>(type: "integer", nullable: false),
                    clp_prix = table.Column<decimal>(type: "numeric", nullable: false),
                    clp_codebarre = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clp", x => new { x.clp_produit, x.clp_couleur });
                    table.ForeignKey(
                        name: "fk_clp_clr",
                        column: x => x.clp_couleur,
                        principalTable: "t_e_couleur_clr",
                        principalColumn: "clr_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_clp_pdt",
                        column: x => x.clp_produit,
                        principalTable: "t_e_produit_pdt",
                        principalColumn: "pdt_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_e_typeproduit_tpd_tpd_surtype",
                table: "t_e_typeproduit_tpd",
                column: "tpd_surtype");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_couleurproduit_clp_clp_couleur",
                table: "t_j_couleurproduit_clp",
                column: "clp_couleur");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_typeproduit_tpd");

            migrationBuilder.DropTable(
                name: "t_j_couleurproduit_clp");

            migrationBuilder.DropTable(
                name: "t_e_couleur_clr");

            migrationBuilder.DropTable(
                name: "t_e_produit_pdt");
        }
    }
}
