﻿// <auto-generated />
using System;
using FifApi.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FifApi.Migrations
{
    [DbContext(typeof(FifaDBContext))]
    partial class FifaDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FifApi.Models.EntityFramework.Adresse", b =>
                {
                    b.Property<int>("IdAdresse")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("adr_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdAdresse"));

                    b.Property<string>("CodePostal")
                        .IsRequired()
                        .HasColumnType("char(15)")
                        .HasColumnName("adr_codepostal");

                    b.Property<int>("IdVille")
                        .HasColumnType("integer")
                        .HasColumnName("adr_ville");

                    b.Property<decimal?>("Lattitude")
                        .HasColumnType("numeric(20,15)")
                        .HasColumnName("adr_lat");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("numeric(20,15)")
                        .HasColumnName("adr_long");

                    b.Property<int?>("NumRue")
                        .HasColumnType("integer")
                        .HasColumnName("adr_numrue");

                    b.Property<string>("Rue")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("adr_rue");

                    b.HasKey("IdAdresse")
                        .HasName("pk_adr");

                    b.HasIndex("IdVille");

                    b.ToTable("t_e_adresse_adr");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Album", b =>
                {
                    b.Property<int>("IdAlbum")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("alb_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdAlbum"));

                    b.Property<string>("NomAlbum")
                        .HasColumnType("text")
                        .HasColumnName("alb_nom");

                    b.HasKey("IdAlbum")
                        .HasName("pk_alb");

                    b.ToTable("t_e_album_alb");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Commande", b =>
                {
                    b.Property<int>("IdCommande")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("cmd_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdCommande"));

                    b.Property<int>("IdUtilisateur")
                        .HasColumnType("integer")
                        .HasColumnName("mcd_utilisateur");

                    b.HasKey("IdCommande")
                        .HasName("pk_cmd");

                    b.HasIndex("IdUtilisateur");

                    b.ToTable("t_e_commande_cmd");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Couleur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("clr_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("clr_nom");

                    b.HasKey("Id")
                        .HasName("pk_clr");

                    b.ToTable("t_e_couleur_clr");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.CouleurProduit", b =>
                {
                    b.Property<int>("IdCouleurProduit")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("clp_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdCouleurProduit"));

                    b.Property<string>("CodeBarre")
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("clp_codebarre");

                    b.Property<int>("IdCouleur")
                        .HasColumnType("integer")
                        .HasColumnName("clp_couleur");

                    b.Property<int>("IdProduit")
                        .HasColumnType("integer")
                        .HasColumnName("clp_produit");

                    b.Property<decimal>("Prix")
                        .HasColumnType("numeric(8,2)")
                        .HasColumnName("clp_prix");

                    b.HasKey("IdCouleurProduit")
                        .HasName("pk_clp");

                    b.HasIndex("IdCouleur");

                    b.HasIndex("IdProduit");

                    b.ToTable("t_j_couleurproduit_clp");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Equipe", b =>
                {
                    b.Property<int>("IdEquipe")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("eqp_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdEquipe"));

                    b.Property<string>("HistoireEquipe")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("eqp_histoire");

                    b.Property<string>("IdPays")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("eqp_pays");

                    b.Property<string>("NomEquipe")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("eqp_nom");

                    b.HasKey("IdEquipe")
                        .HasName("pk_eqp");

                    b.HasIndex("IdPays");

                    b.ToTable("t_e_equipe_eqp");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.InfoCB", b =>
                {
                    b.Property<int>("IdCB")
                        .HasColumnType("integer")
                        .HasColumnName("icb_id");

                    b.Property<decimal>("Cryptogramme")
                        .HasColumnType("numeric(3,0)")
                        .HasColumnName("icb_cryptogramme");

                    b.Property<DateTime>("DateExpiration")
                        .HasColumnType("date")
                        .HasColumnName("icb_dateexpiration");

                    b.Property<decimal>("NumeroCB")
                        .HasColumnType("numeric(16,0)")
                        .HasColumnName("icb_numcb");

                    b.Property<int>("UtilisateurId")
                        .HasColumnType("integer")
                        .HasColumnName("icb_utilisateur");

                    b.HasKey("IdCB")
                        .HasName("pk_icb");

                    b.ToTable("t_e_infocb_icb");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Joueur", b =>
                {
                    b.Property<int>("IdJoueur")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("jor_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdJoueur"));

                    b.Property<DateTime?>("DateDecesJoueur")
                        .HasColumnType("Date")
                        .HasColumnName("jor_datedece");

                    b.Property<DateTime?>("DateNaissanceJoueur")
                        .HasColumnType("Date")
                        .HasColumnName("jor_datenaissance");

                    b.Property<DateTime?>("DebutCarriereJoueur")
                        .HasColumnType("Date")
                        .HasColumnName("jor_debutcarriere");

                    b.Property<string>("DescriptionJoueur")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("jor_description");

                    b.Property<DateTime?>("FinCarriereJoueur")
                        .HasColumnType("Date")
                        .HasColumnName("jor_fincarriere");

                    b.Property<string>("NomJoueur")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("jor_nom");

                    b.Property<int>("PosteId")
                        .HasColumnType("int")
                        .HasColumnName("posteid");

                    b.Property<string>("PrenomJoueur")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("prenomjoueur");

                    b.Property<string>("SexeJoueur")
                        .IsRequired()
                        .HasColumnType("Char(1)")
                        .HasColumnName("jor_sexe");

                    b.HasKey("IdJoueur")
                        .HasName("pk_jor");

                    b.HasIndex("PosteId");

                    b.ToTable("t_e_joueur_jor");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.JoueurMatch", b =>
                {
                    b.Property<int>("MatchId")
                        .HasColumnType("integer")
                        .HasColumnName("jrm_match");

                    b.Property<int>("JoueurId")
                        .HasColumnType("integer")
                        .HasColumnName("jrm_joueur");

                    b.Property<int>("NbButs")
                        .HasColumnType("integer")
                        .HasColumnName("jrm_nbbut");

                    b.HasKey("MatchId", "JoueurId")
                        .HasName("pk_jrm");

                    b.HasIndex("JoueurId");

                    b.ToTable("t_j_joueurmatch_jrm");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.LigneCommande", b =>
                {
                    b.Property<int>("IdCommande")
                        .HasColumnType("integer")
                        .HasColumnName("lcm_commande");

                    b.Property<int>("IdStock")
                        .HasColumnType("integer")
                        .HasColumnName("lcm_commande");

                    b.HasKey("IdCommande", "IdStock")
                        .HasName("pk_lcm");

                    b.HasIndex("IdStock");

                    b.ToTable("t_j_lignecommande_lcm");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Marque", b =>
                {
                    b.Property<int>("IdMarque")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("mrq_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdMarque"));

                    b.Property<string>("NomMarque")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("mrq_nom");

                    b.HasKey("IdMarque")
                        .HasName("pk_mrq");

                    b.ToTable("t_e_marque_mrq");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Match", b =>
                {
                    b.Property<int>("IdMatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("mch_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdMatch"));

                    b.Property<DateTime?>("DateMatch")
                        .HasColumnType("Date")
                        .HasColumnName("mch_date");

                    b.Property<int>("IdEquipe")
                        .HasColumnType("integer")
                        .HasColumnName("mch_equipe");

                    b.Property<string>("NomMatch")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("mch_nom");

                    b.Property<int?>("ScoreEquipeDomicile")
                        .HasColumnType("integer")
                        .HasColumnName("mch_scoreint");

                    b.Property<int?>("ScoreEquipeExterieure")
                        .HasColumnType("integer")
                        .HasColumnName("mch_scoreext");

                    b.HasKey("IdMatch")
                        .HasName("pk_mch");

                    b.HasIndex("IdEquipe");

                    b.ToTable("t_e_match_mch");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Pays", b =>
                {
                    b.Property<string>("IdPays")
                        .HasColumnType("text")
                        .HasColumnName("pay_id");

                    b.Property<int?>("NomPays")
                        .HasMaxLength(75)
                        .HasColumnType("integer")
                        .HasColumnName("pay_nom");

                    b.HasKey("IdPays")
                        .HasName("pk_pay");

                    b.ToTable("t_e_pays_pay");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Poste", b =>
                {
                    b.Property<int>("Idposte")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("pst_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Idposte"));

                    b.Property<string>("DescriptionPoste")
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("pst_description");

                    b.Property<string>("NomPoste")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("pst_nom");

                    b.HasKey("Idposte")
                        .HasName("pk_pst");

                    b.ToTable("t_e_poste_pst");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Produit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("pdt_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AlbumId")
                        .HasColumnType("int")
                        .HasColumnName("pdt_album");

                    b.Property<string>("Caracteristiques")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("pdt_caracteristiques");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("pdt_description");

                    b.Property<int>("MarqueId")
                        .HasColumnType("int")
                        .HasColumnName("pdt_marque");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("pdt_nom");

                    b.Property<int>("TypeId")
                        .HasColumnType("int")
                        .HasColumnName("pdt_type");

                    b.HasKey("Id")
                        .HasName("pk_pdt");

                    b.HasIndex("AlbumId");

                    b.HasIndex("MarqueId");

                    b.HasIndex("TypeId");

                    b.ToTable("t_e_produit_pdt");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Sponsor", b =>
                {
                    b.Property<int>("IdMarque")
                        .HasColumnType("integer")
                        .HasColumnName("spc_marque");

                    b.Property<int>("IdEquipe")
                        .HasColumnType("integer")
                        .HasColumnName("spc_equipe");

                    b.HasKey("IdMarque", "IdEquipe")
                        .HasName("pk_spc");

                    b.HasIndex("IdEquipe");

                    b.ToTable("t_j_sponsor_spc");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Stock", b =>
                {
                    b.Property<int>("IdStock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("stc_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdStock"));

                    b.Property<int>("CouleurProduitId")
                        .HasColumnType("int")
                        .HasColumnName("stc_couleurproduit");

                    b.Property<int>("Quantite")
                        .HasColumnType("integer")
                        .HasColumnName("stc_quantite");

                    b.Property<string>("TailleId")
                        .IsRequired()
                        .HasColumnType("char(6)")
                        .HasColumnName("stc_taille");

                    b.HasKey("IdStock")
                        .HasName("pk_stc");

                    b.HasIndex("CouleurProduitId");

                    b.HasIndex("TailleId");

                    b.ToTable("t_e_stock_stc");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Taille", b =>
                {
                    b.Property<string>("IdTaille")
                        .HasColumnType("char(6)")
                        .HasColumnName("tal_taille");

                    b.Property<string>("DescriptionTaille")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("tal_description");

                    b.Property<string>("NomTaille")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("tal_nom");

                    b.HasKey("IdTaille")
                        .HasName("pk_tal");

                    b.ToTable("t_e_taille_tal");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.TypeProduit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("tpd_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("tpd_description");

                    b.Property<int?>("IdSurType")
                        .HasColumnType("integer")
                        .HasColumnName("tpd_surtype");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("tpd_nom");

                    b.HasKey("Id")
                        .HasName("pk_tpd");

                    b.HasIndex("IdSurType");

                    b.ToTable("t_e_typeproduit_tpd");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Utilisateur", b =>
                {
                    b.Property<int>("IdUtilisateur")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("utl_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdUtilisateur"));

                    b.Property<int>("IdAdresse")
                        .HasColumnType("integer")
                        .HasColumnName("utl_adresse");

                    b.Property<string>("MailUtilisateur")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("utl_email");

                    b.Property<string>("NomUtilisateur")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("utl_nom");

                    b.Property<string>("PrenomUtilisateur")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("utl_prenom");

                    b.Property<string>("PseudoUtilisateur")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("utl_pseudo");

                    b.HasKey("IdUtilisateur")
                        .HasName("pk_utl");

                    b.HasIndex("IdAdresse");

                    b.ToTable("t_e_utilisateur_utl");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Ville", b =>
                {
                    b.Property<int>("IdVille")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("vil_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdVille"));

                    b.Property<string>("IdPays")
                        .IsRequired()
                        .HasColumnType("char(1)")
                        .HasColumnName("vil_pays");

                    b.Property<string>("NomVille")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("vil_nom");

                    b.Property<string>("NumDep")
                        .HasColumnType("char(1)")
                        .HasColumnName("vil_numdep");

                    b.HasKey("IdVille")
                        .HasName("pk_vil");

                    b.HasIndex("IdPays");

                    b.ToTable("t_e_ville_vil");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Adresse", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Ville", "VilleAdresse")
                        .WithMany("AdresseDeLaVille")
                        .HasForeignKey("IdVille")
                        .IsRequired()
                        .HasConstraintName("fk_adr_vil");

                    b.Navigation("VilleAdresse");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Commande", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Utilisateur", "UtilisateurCommande")
                        .WithMany("CommandeDeUtilisateur")
                        .HasForeignKey("IdUtilisateur")
                        .IsRequired()
                        .HasConstraintName("fk_cmd_utl");

                    b.Navigation("UtilisateurCommande");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.CouleurProduit", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Couleur", "Couleur_CouleurProduit")
                        .WithMany("CouleurProduits")
                        .HasForeignKey("IdCouleur")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_clp_clr");

                    b.HasOne("FifApi.Models.EntityFramework.Produit", "Produit_CouleurProduit")
                        .WithMany("CouleursProduits")
                        .HasForeignKey("IdProduit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_clp_pdt");

                    b.Navigation("Couleur_CouleurProduit");

                    b.Navigation("Produit_CouleurProduit");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Equipe", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Pays", "PaysDeEquipe")
                        .WithMany("EquipeDuPays")
                        .HasForeignKey("IdPays")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_eqp_pay");

                    b.Navigation("PaysDeEquipe");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.InfoCB", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Utilisateur", "UtilisateurCB")
                        .WithMany("CBDeUtilisateur")
                        .HasForeignKey("IdCB")
                        .IsRequired()
                        .HasConstraintName("fk_icb_utl");

                    b.Navigation("UtilisateurCB");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Joueur", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Poste", "PostePourJoueur")
                        .WithMany("JoueurPoste")
                        .HasForeignKey("PosteId")
                        .IsRequired()
                        .HasConstraintName("fk_jor_pst");

                    b.Navigation("PostePourJoueur");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.JoueurMatch", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Joueur", "JoueurDansMatch")
                        .WithMany("JouabiliteMatch")
                        .HasForeignKey("JoueurId")
                        .IsRequired()
                        .HasConstraintName("fk_jrm_jor");

                    b.HasOne("FifApi.Models.EntityFramework.Match", "MatchPourJoueur")
                        .WithMany("JouabiliteMatch")
                        .HasForeignKey("MatchId")
                        .IsRequired()
                        .HasConstraintName("fk_jrm_mch");

                    b.Navigation("JoueurDansMatch");

                    b.Navigation("MatchPourJoueur");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.LigneCommande", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Commande", "CommandeLigne")
                        .WithMany("LigneDeLaCommande")
                        .HasForeignKey("IdCommande")
                        .IsRequired()
                        .HasConstraintName("fk_lcm_cmd");

                    b.HasOne("FifApi.Models.EntityFramework.Stock", "StockLigneCommande")
                        .WithMany("LigneDuStock")
                        .HasForeignKey("IdStock")
                        .IsRequired()
                        .HasConstraintName("fk_lcm_stc");

                    b.Navigation("CommandeLigne");

                    b.Navigation("StockLigneCommande");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Match", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Equipe", "EquipeEnMatch")
                        .WithMany("MatchEnEquipe")
                        .HasForeignKey("IdEquipe")
                        .IsRequired()
                        .HasConstraintName("fk_mch_eqp");

                    b.Navigation("EquipeEnMatch");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Produit", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Album", "AlbumDuProduit")
                        .WithMany("ProduitAlbum")
                        .HasForeignKey("AlbumId")
                        .IsRequired()
                        .HasConstraintName("fk_pdt_alb");

                    b.HasOne("FifApi.Models.EntityFramework.Marque", "MarqueduProduit")
                        .WithMany("ProduitMarque")
                        .HasForeignKey("MarqueId")
                        .IsRequired()
                        .HasConstraintName("fk_pdt_mrq");

                    b.HasOne("FifApi.Models.EntityFramework.TypeProduit", "TypePourLeProduit")
                        .WithMany("TypographieDuProduit")
                        .HasForeignKey("TypeId")
                        .IsRequired()
                        .HasConstraintName("fk_pdt_tpd");

                    b.Navigation("AlbumDuProduit");

                    b.Navigation("MarqueduProduit");

                    b.Navigation("TypePourLeProduit");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Sponsor", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Equipe", "EquipeSponsorise")
                        .WithMany("SponsorMarque")
                        .HasForeignKey("IdEquipe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_spc_eqp");

                    b.HasOne("FifApi.Models.EntityFramework.Marque", "MarqueDuSponsor")
                        .WithMany("SponsorMarque")
                        .HasForeignKey("IdMarque")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_spc_mrq");

                    b.Navigation("EquipeSponsorise");

                    b.Navigation("MarqueDuSponsor");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Stock", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.CouleurProduit", "ProduitEncouleur")
                        .WithMany("ProduitStock")
                        .HasForeignKey("CouleurProduitId")
                        .IsRequired()
                        .HasConstraintName("fk_stc_clp");

                    b.HasOne("FifApi.Models.EntityFramework.Taille", "TailleDuProduit")
                        .WithMany("StockDuProduit")
                        .HasForeignKey("TailleId")
                        .IsRequired()
                        .HasConstraintName("fk_stc_tal");

                    b.Navigation("ProduitEncouleur");

                    b.Navigation("TailleDuProduit");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.TypeProduit", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.TypeProduit", "SurType")
                        .WithMany("SousTypes")
                        .HasForeignKey("IdSurType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_tpd_tpd");

                    b.Navigation("SurType");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Utilisateur", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Adresse", "AdresseDeUtilisateur")
                        .WithMany("UtilisateurAdresse")
                        .HasForeignKey("IdAdresse")
                        .IsRequired()
                        .HasConstraintName("fk_utl_adr");

                    b.Navigation("AdresseDeUtilisateur");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Ville", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Pays", "PaysVille")
                        .WithMany("VilleDuPays")
                        .HasForeignKey("IdPays")
                        .IsRequired()
                        .HasConstraintName("fk_vil_pay");

                    b.Navigation("PaysVille");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Adresse", b =>
                {
                    b.Navigation("UtilisateurAdresse");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Album", b =>
                {
                    b.Navigation("ProduitAlbum");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Commande", b =>
                {
                    b.Navigation("LigneDeLaCommande");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Couleur", b =>
                {
                    b.Navigation("CouleurProduits");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.CouleurProduit", b =>
                {
                    b.Navigation("ProduitStock");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Equipe", b =>
                {
                    b.Navigation("MatchEnEquipe");

                    b.Navigation("SponsorMarque");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Joueur", b =>
                {
                    b.Navigation("JouabiliteMatch");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Marque", b =>
                {
                    b.Navigation("ProduitMarque");

                    b.Navigation("SponsorMarque");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Match", b =>
                {
                    b.Navigation("JouabiliteMatch");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Pays", b =>
                {
                    b.Navigation("EquipeDuPays");

                    b.Navigation("VilleDuPays");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Poste", b =>
                {
                    b.Navigation("JoueurPoste");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Produit", b =>
                {
                    b.Navigation("CouleursProduits");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Stock", b =>
                {
                    b.Navigation("LigneDuStock");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Taille", b =>
                {
                    b.Navigation("StockDuProduit");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.TypeProduit", b =>
                {
                    b.Navigation("SousTypes");

                    b.Navigation("TypographieDuProduit");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Utilisateur", b =>
                {
                    b.Navigation("CBDeUtilisateur");

                    b.Navigation("CommandeDeUtilisateur");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Ville", b =>
                {
                    b.Navigation("AdresseDeLaVille");
                });
#pragma warning restore 612, 618
        }
    }
}
