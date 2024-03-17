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
                    table.PrimaryKey("pk_alb", x => x.alb_id);
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
                    table.PrimaryKey("pk_mrq", x => x.mrq_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_pays_pay",
                columns: table => new
                {
                    pay_id = table.Column<string>(type: "text", nullable: false),
                    pay_nom = table.Column<int>(type: "integer", maxLength: 75, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pay", x => x.pay_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_photo_pht",
                columns: table => new
                {
                    pht_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pht_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    pht_titre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    pht_description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pht", x => x.pht_id);
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
                    table.PrimaryKey("pk_pst", x => x.pst_id);
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
                    table.PrimaryKey("pk_tal", x => x.tal_taille);
                });

            migrationBuilder.CreateTable(
                name: "t_e_typeproduit_tpd",
                columns: table => new
                {
                    tpd_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tpd_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tpd_description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    tpd_surtype = table.Column<int>(type: "integer", nullable: true)
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
                name: "t_e_equipe_eqp",
                columns: table => new
                {
                    eqp_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    eqp_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    eqp_histoire = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    eqp_pays = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_eqp", x => x.eqp_id);
                    table.ForeignKey(
                        name: "fk_eqp_pay",
                        column: x => x.eqp_pays,
                        principalTable: "t_e_pays_pay",
                        principalColumn: "pay_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_ville_vil",
                columns: table => new
                {
                    vil_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vil_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    vil_numdep = table.Column<string>(type: "char(1)", nullable: true),
                    vil_pays = table.Column<string>(type: "char(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vil", x => x.vil_id);
                    table.ForeignKey(
                        name: "fk_vil_pay",
                        column: x => x.vil_pays,
                        principalTable: "t_e_pays_pay",
                        principalColumn: "pay_id");
                });

            migrationBuilder.CreateTable(
                name: "t_j_albumphoto_abp",
                columns: table => new
                {
                    abp_album = table.Column<int>(type: "integer", nullable: false),
                    abp_photo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_abp", x => new { x.abp_photo, x.abp_album });
                    table.ForeignKey(
                        name: "fk_abp_alb",
                        column: x => x.abp_album,
                        principalTable: "t_e_album_alb",
                        principalColumn: "alb_id");
                    table.ForeignKey(
                        name: "fk_abp_pht",
                        column: x => x.abp_photo,
                        principalTable: "t_e_photo_pht",
                        principalColumn: "pht_id");
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
                name: "t_e_match_mch",
                columns: table => new
                {
                    mch_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mch_equipe = table.Column<int>(type: "integer", nullable: false),
                    mch_scoreint = table.Column<int>(type: "integer", nullable: true),
                    mch_scoreext = table.Column<int>(type: "integer", nullable: true),
                    mch_nom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mch_date = table.Column<DateTime>(type: "Date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mch", x => x.mch_id);
                    table.ForeignKey(
                        name: "fk_mch_eqp",
                        column: x => x.mch_equipe,
                        principalTable: "t_e_equipe_eqp",
                        principalColumn: "eqp_id");
                });

            migrationBuilder.CreateTable(
                name: "t_j_sponsor_spc",
                columns: table => new
                {
                    spc_equipe = table.Column<int>(type: "integer", nullable: false),
                    spc_marque = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_spc", x => new { x.spc_marque, x.spc_equipe });
                    table.ForeignKey(
                        name: "fk_spc_eqp",
                        column: x => x.spc_equipe,
                        principalTable: "t_e_equipe_eqp",
                        principalColumn: "eqp_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_spc_mrq",
                        column: x => x.spc_marque,
                        principalTable: "t_e_marque_mrq",
                        principalColumn: "mrq_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_adresse_adr",
                columns: table => new
                {
                    adr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adr_codepostal = table.Column<string>(type: "char(15)", nullable: false),
                    adr_rue = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    adr_numrue = table.Column<int>(type: "integer", nullable: true),
                    adr_long = table.Column<decimal>(type: "numeric(20,15)", nullable: true),
                    adr_lat = table.Column<decimal>(type: "numeric(20,15)", nullable: true),
                    adr_ville = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adr", x => x.adr_id);
                    table.ForeignKey(
                        name: "fk_adr_vil",
                        column: x => x.adr_ville,
                        principalTable: "t_e_ville_vil",
                        principalColumn: "vil_id");
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
                name: "t_e_utilisateur_utl",
                columns: table => new
                {
                    utl_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    utl_pseudo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    utl_mdp = table.Column<string>(type: "text", nullable: false),
                    utl_nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    utl_prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    utl_email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    utl_adresse = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_utl", x => x.utl_id);
                    table.ForeignKey(
                        name: "fk_utl_adr",
                        column: x => x.utl_adresse,
                        principalTable: "t_e_adresse_adr",
                        principalColumn: "adr_id");
                });

            migrationBuilder.CreateTable(
                name: "t_e_stock_stc",
                columns: table => new
                {
                    stc_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stc_taille = table.Column<string>(type: "char(6)", nullable: false),
                    stc_quantite = table.Column<int>(type: "integer", nullable: false),
                    stc_couleurproduit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stc", x => x.stc_id);
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

            migrationBuilder.CreateTable(
                name: "t_e_commande_cmd",
                columns: table => new
                {
                    cmd_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mcd_utilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cmd", x => x.cmd_id);
                    table.ForeignKey(
                        name: "fk_cmd_utl",
                        column: x => x.mcd_utilisateur,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id");
                });

            migrationBuilder.CreateTable(
                name: "t_e_infocb_icb",
                columns: table => new
                {
                    icb_id = table.Column<int>(type: "integer", nullable: false),
                    icb_numcb = table.Column<decimal>(type: "numeric(16,0)", nullable: false),
                    icb_cryptogramme = table.Column<decimal>(type: "numeric(3,0)", nullable: false),
                    icb_dateexpiration = table.Column<DateTime>(type: "date", nullable: false),
                    icb_utilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_icb", x => x.icb_id);
                    table.ForeignKey(
                        name: "fk_icb_utl",
                        column: x => x.icb_id,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id");
                });

            migrationBuilder.CreateTable(
                name: "t_j_vote_vot",
                columns: table => new
                {
                    vot_joueur = table.Column<int>(type: "integer", nullable: false),
                    vot_utilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vot", x => new { x.vot_joueur, x.vot_utilisateur });
                    table.ForeignKey(
                        name: "fk_vot_jor",
                        column: x => x.vot_joueur,
                        principalTable: "t_e_joueur_jor",
                        principalColumn: "jor_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vot_utl",
                        column: x => x.vot_utilisateur,
                        principalTable: "t_e_utilisateur_utl",
                        principalColumn: "utl_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_j_lignecommande_lcm",
                columns: table => new
                {
                    lcm_commande = table.Column<int>(type: "integer", nullable: false),
                    lcm_stock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lcm", x => new { x.lcm_commande, x.lcm_stock });
                    table.ForeignKey(
                        name: "fk_lcm_cmd",
                        column: x => x.lcm_commande,
                        principalTable: "t_e_commande_cmd",
                        principalColumn: "cmd_id");
                    table.ForeignKey(
                        name: "fk_lcm_stc",
                        column: x => x.lcm_stock,
                        principalTable: "t_e_stock_stc",
                        principalColumn: "stc_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_e_adresse_adr_adr_ville",
                table: "t_e_adresse_adr",
                column: "adr_ville");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_commande_cmd_mcd_utilisateur",
                table: "t_e_commande_cmd",
                column: "mcd_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_equipe_eqp_eqp_pays",
                table: "t_e_equipe_eqp",
                column: "eqp_pays");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_joueur_jor_posteid",
                table: "t_e_joueur_jor",
                column: "posteid");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_match_mch_mch_equipe",
                table: "t_e_match_mch",
                column: "mch_equipe");

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
                name: "IX_t_e_stock_stc_stc_taille",
                table: "t_e_stock_stc",
                column: "stc_taille");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_typeproduit_tpd_tpd_surtype",
                table: "t_e_typeproduit_tpd",
                column: "tpd_surtype");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_utilisateur_utl_utl_adresse",
                table: "t_e_utilisateur_utl",
                column: "utl_adresse");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_ville_vil_vil_pays",
                table: "t_e_ville_vil",
                column: "vil_pays");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_albumphoto_abp_abp_album",
                table: "t_j_albumphoto_abp",
                column: "abp_album");

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

            migrationBuilder.CreateIndex(
                name: "IX_t_j_lignecommande_lcm_lcm_stock",
                table: "t_j_lignecommande_lcm",
                column: "lcm_stock");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_sponsor_spc_spc_equipe",
                table: "t_j_sponsor_spc",
                column: "spc_equipe");

            migrationBuilder.CreateIndex(
                name: "IX_t_j_vote_vot_vot_utilisateur",
                table: "t_j_vote_vot",
                column: "vot_utilisateur");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_infocb_icb");

            migrationBuilder.DropTable(
                name: "t_j_albumphoto_abp");

            migrationBuilder.DropTable(
                name: "t_j_joueurmatch_jrm");

            migrationBuilder.DropTable(
                name: "t_j_lignecommande_lcm");

            migrationBuilder.DropTable(
                name: "t_j_sponsor_spc");

            migrationBuilder.DropTable(
                name: "t_j_vote_vot");

            migrationBuilder.DropTable(
                name: "t_e_photo_pht");

            migrationBuilder.DropTable(
                name: "t_e_match_mch");

            migrationBuilder.DropTable(
                name: "t_e_commande_cmd");

            migrationBuilder.DropTable(
                name: "t_e_stock_stc");

            migrationBuilder.DropTable(
                name: "t_e_joueur_jor");

            migrationBuilder.DropTable(
                name: "t_e_equipe_eqp");

            migrationBuilder.DropTable(
                name: "t_e_utilisateur_utl");

            migrationBuilder.DropTable(
                name: "t_j_couleurproduit_clp");

            migrationBuilder.DropTable(
                name: "t_e_taille_tal");

            migrationBuilder.DropTable(
                name: "t_e_poste_pst");

            migrationBuilder.DropTable(
                name: "t_e_adresse_adr");

            migrationBuilder.DropTable(
                name: "t_e_couleur_clr");

            migrationBuilder.DropTable(
                name: "t_e_produit_pdt");

            migrationBuilder.DropTable(
                name: "t_e_ville_vil");

            migrationBuilder.DropTable(
                name: "t_e_album_alb");

            migrationBuilder.DropTable(
                name: "t_e_marque_mrq");

            migrationBuilder.DropTable(
                name: "t_e_typeproduit_tpd");

            migrationBuilder.DropTable(
                name: "t_e_pays_pay");
        }
    }
}
