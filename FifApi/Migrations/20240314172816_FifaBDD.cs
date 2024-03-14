using System;
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
                name: "t_e_album_alb",
                columns: table => new
                {
                    alb_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alb_nom = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_album_alb", x => x.alb_id);
                });

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
                name: "t_e_marque_mrq",
                columns: table => new
                {
                    mrq_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mrq_nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_marque_mrq", x => x.mrq_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_match_mch",
                columns: table => new
                {
                    mch_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mch_scoreint = table.Column<int>(type: "integer", nullable: true),
                    mch_scoreext = table.Column<int>(type: "integer", nullable: true),
                    mch_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mch_date = table.Column<DateTime>(type: "Date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_match_mch", x => x.mch_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_poste_pst",
                columns: table => new
                {
                    pst_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pst_nom = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    pst_description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_poste_pst", x => x.pst_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_taille_tal",
                columns: table => new
                {
                    tal_taille = table.Column<string>(type: "char(6)", nullable: false),
                    tal_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tal_description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_taille_tal", x => x.tal_taille);
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
                name: "t_e_joueur_jor",
                columns: table => new
                {
                    jor_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    jor_nom = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    prenomjoueur = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    jor_sexe = table.Column<string>(type: "Char(1)", nullable: false),
                    jor_datenaissance = table.Column<DateTime>(type: "Date", nullable: true),
                    jor_datedece = table.Column<DateTime>(type: "Date", nullable: true),
                    jor_debutcarriere = table.Column<DateTime>(type: "Date", nullable: true),
                    jor_fincarriere = table.Column<DateTime>(type: "Date", nullable: true),
                    jor_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    posteid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jor", x => x.jor_id);
                    table.ForeignKey(
                        name: "fk_jor_pst",
                        column: x => x.posteid,
                        principalTable: "t_e_poste_pst",
                        principalColumn: "pst_id");
                });

            migrationBuilder.CreateTable(
                name: "t_e_produit_pdt",
                columns: table => new
                {
                    pdt_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pdt_nom = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    pdt_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    pdt_caracteristiques = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    pdt_marque = table.Column<int>(type: "int", nullable: false),
                    pdt_type = table.Column<int>(type: "int", nullable: false),
                    pdt_album = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pdt", x => x.pdt_id);
                    table.ForeignKey(
                        name: "fk_pdt_alb",
                        column: x => x.pdt_album,
                        principalTable: "t_e_album_alb",
                        principalColumn: "alb_id");
                    table.ForeignKey(
                        name: "fk_pdt_mrq",
                        column: x => x.pdt_marque,
                        principalTable: "t_e_marque_mrq",
                        principalColumn: "mrq_id");
                    table.ForeignKey(
                        name: "fk_pdt_tpd",
                        column: x => x.pdt_type,
                        principalTable: "t_e_typeproduit_tpd",
                        principalColumn: "tpd_id");
                });

            migrationBuilder.CreateTable(
                name: "t_j_joueurmatch_jrm",
                columns: table => new
                {
                    jrm_joueur = table.Column<int>(type: "integer", nullable: false),
                    jrm_match = table.Column<int>(type: "integer", nullable: false),
                    jrm_nbbut = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jrm", x => new { x.jrm_match, x.jrm_joueur });
                    table.ForeignKey(
                        name: "fk_jrm_jor",
                        column: x => x.jrm_joueur,
                        principalTable: "t_e_joueur_jor",
                        principalColumn: "jor_id");
                    table.ForeignKey(
                        name: "fk_jrm_mch",
                        column: x => x.jrm_match,
                        principalTable: "t_e_match_mch",
                        principalColumn: "mch_id");
                });

            migrationBuilder.CreateTable(
                name: "t_j_couleurproduit_clp",
                columns: table => new
                {
                    clp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    clp_produit = table.Column<int>(type: "integer", nullable: false),
                    clp_couleur = table.Column<int>(type: "integer", nullable: false),
                    clp_prix = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    clp_codebarre = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clp", x => x.clp_id);
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

            migrationBuilder.CreateTable(
                name: "t_e_stock_stc",
                columns: table => new
                {
                    stc_taille = table.Column<string>(type: "char(6)", nullable: false),
                    stc_couleurproduit = table.Column<int>(type: "int", nullable: false),
                    stc_quantite = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stc", x => new { x.stc_taille, x.stc_couleurproduit });
                    table.ForeignKey(
                        name: "fk_stc_clp",
                        column: x => x.stc_couleurproduit,
                        principalTable: "t_j_couleurproduit_clp",
                        principalColumn: "clp_id");
                    table.ForeignKey(
                        name: "fk_stc_tal",
                        column: x => x.stc_taille,
                        principalTable: "t_e_taille_tal",
                        principalColumn: "tal_taille");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_e_joueur_jor_posteid",
                table: "t_e_joueur_jor",
                column: "posteid");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_pdt_pdt_album",
                table: "t_e_produit_pdt",
                column: "pdt_album");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_pdt_pdt_marque",
                table: "t_e_produit_pdt",
                column: "pdt_marque");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_produit_pdt_pdt_type",
                table: "t_e_produit_pdt",
                column: "pdt_type");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_stock_stc_stc_couleurproduit",
                table: "t_e_stock_stc",
                column: "stc_couleurproduit");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_typeproduit_tpd_tpd_surtype",
                table: "t_e_typeproduit_tpd",
                column: "tpd_surtype");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_couleurproduit_clp_clp_couleur",
                table: "t_j_couleurproduit_clp",
                column: "clp_couleur");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_couleurproduit_clp_clp_produit",
                table: "t_j_couleurproduit_clp",
                column: "clp_produit");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_joueurmatch_jrm_jrm_joueur",
                table: "t_j_joueurmatch_jrm",
                column: "jrm_joueur");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_stock_stc");

            migrationBuilder.DropTable(
                name: "t_j_joueurmatch_jrm");

            migrationBuilder.DropTable(
                name: "t_j_couleurproduit_clp");

            migrationBuilder.DropTable(
                name: "t_e_taille_tal");

            migrationBuilder.DropTable(
                name: "t_e_joueur_jor");

            migrationBuilder.DropTable(
                name: "t_e_match_mch");

            migrationBuilder.DropTable(
                name: "t_e_couleur_clr");

            migrationBuilder.DropTable(
                name: "t_e_produit_pdt");

            migrationBuilder.DropTable(
                name: "t_e_poste_pst");

            migrationBuilder.DropTable(
                name: "t_e_album_alb");

            migrationBuilder.DropTable(
                name: "t_e_marque_mrq");

            migrationBuilder.DropTable(
                name: "t_e_typeproduit_tpd");
        }
    }
}
