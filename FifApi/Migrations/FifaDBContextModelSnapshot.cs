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
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

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
                    b.Property<int>("IdProduit")
                        .HasColumnType("integer")
                        .HasColumnName("clp_produit");

                    b.Property<int>("IdCouleur")
                        .HasColumnType("integer")
                        .HasColumnName("clp_couleur");

                    b.Property<string>("CodeBarre")
                        .HasMaxLength(48)
                        .HasColumnType("character varying(48)")
                        .HasColumnName("clp_codebarre");

                    b.Property<decimal>("Prix")
                        .HasColumnType("numeric")
                        .HasColumnName("clp_prix");

                    b.HasKey("IdProduit", "IdCouleur")
                        .HasName("pk_clp");

                    b.HasIndex("IdCouleur");

                    b.ToTable("t_j_couleurproduit_clp");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Produit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("pdt_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Caracteristiques")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("pdt_caracteristiques");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("pdt_description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("pdt_nom");

                    b.HasKey("Id")
                        .HasName("pk_pdt");

                    b.ToTable("t_e_produit_pdt");
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
                        .IsRequired()
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

            modelBuilder.Entity("FifApi.Models.EntityFramework.CouleurProduit", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.Couleur", "Couleur_CouleurProduit")
                        .WithMany("CouleurProduits")
                        .HasForeignKey("IdCouleur")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_clp_clr");

                    b.HasOne("FifApi.Models.EntityFramework.Produit", "Produit_CouleurProduit")
                        .WithMany("CouleursProduit")
                        .HasForeignKey("IdProduit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_clp_pdt");

                    b.Navigation("Couleur_CouleurProduit");

                    b.Navigation("Produit_CouleurProduit");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.TypeProduit", b =>
                {
                    b.HasOne("FifApi.Models.EntityFramework.TypeProduit", "SurType")
                        .WithMany("SousTypes")
                        .HasForeignKey("IdSurType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tpd_tpd");

                    b.Navigation("SurType");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Couleur", b =>
                {
                    b.Navigation("CouleurProduits");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.Produit", b =>
                {
                    b.Navigation("CouleursProduit");
                });

            modelBuilder.Entity("FifApi.Models.EntityFramework.TypeProduit", b =>
                {
                    b.Navigation("SousTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
