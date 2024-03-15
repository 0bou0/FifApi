using Microsoft.EntityFrameworkCore;

namespace FifApi.Models.EntityFramework
{
    public partial class FifaDBContext : DbContext
    {
        public FifaDBContext() { }
        public FifaDBContext(DbContextOptions<FifaDBContext> options)
            : base(options) { }


        public virtual DbSet<JoueurMatch> JoueurMatchs { get; set; } = null!;
        public virtual DbSet<Joueur> Joueurs { get; set; } = null!;
        public virtual DbSet<Match> Matchs { get; set; } = null!;
        public virtual DbSet<Poste> Postes { get; set; } = null!;
        public virtual DbSet<Produit> Produits { get; set; } = null!;
        public virtual DbSet<Marque> Marques { get; set; } = null!;
        public virtual DbSet<Couleur> Couleurs { get; set; } = null!;
        public virtual DbSet<CouleurProduit> CouleurProduits { get; set; } = null!;
        public virtual DbSet<Taille> Tailles { get; set; } = null!;
        public virtual DbSet<Stock> Stocks { get; set; } = null!;
        public virtual DbSet<TypeProduit> TypeProduits { get; set; } = null!;
        public virtual DbSet<Sponsor> Sponsors { get; set; } = null!;
        public virtual DbSet<Equipe> Equipes { get; set; } = null!;
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
        public virtual DbSet<Adresse> Adresses { get; set; } = null!;
        public virtual DbSet<Ville> Villes { get; set; } = null!;
        public virtual DbSet<Pays> Pays { get; set; } = null!;
        public virtual DbSet<InfoCB> InfoCBs { get; set; } = null!;
        public virtual DbSet<Commande> Commandes { get; set; } = null!;
        public virtual DbSet<LigneCommande> LigneCommandes { get; set; } = null!;
        public virtual DbSet<Album> Albums { get; set; } = null!;



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("Server=localhost;port=5432;Database=FifaBDD; uid=postgres; password=postgres;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<JoueurMatch>(entity =>
            {
                entity.HasKey(e => new { e.MatchId, e.JoueurId })
                    .HasName("pk_jrm");

                entity.HasOne(d => d.MatchPourJoueur)
                    .WithMany(p => p.JouabiliteMatch)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_jrm_mch");

                entity.HasOne(d => d.JoueurDansMatch)
                    .WithMany(p => p.JouabiliteMatch)
                    .HasForeignKey(d => d.JoueurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_jrm_jor");
            });

            modelBuilder.Entity<Joueur>(entity =>
            {
                entity.HasKey(e => new { e.IdJoueur })
                   .HasName("pk_jor");

                entity.HasOne(d => d.PostePourJoueur)
                    .WithMany(p => p.JoueurPoste)
                    .HasForeignKey(d => d.PosteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_jor_pst");
            });


            modelBuilder.Entity<Produit>(entity =>
            {
                entity.HasKey(x => x.Id)
                    .HasName("pk_pdt");

                entity.HasOne(d => d.MarqueduProduit)
                   .WithMany(p => p.ProduitMarque)
                   .HasForeignKey(d => d.MarqueId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("fk_pdt_mrq");

                entity.HasOne(d => d.TypePourLeProduit)
                   .WithMany(p => p.TypographieDuProduit)
                   .HasForeignKey(d => d.TypeId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("fk_pdt_tpd");

                entity.HasOne(d => d.AlbumDuProduit)
                 .WithMany(p => p.ProduitAlbum)
                 .HasForeignKey(d => d.AlbumId)
                 .OnDelete(DeleteBehavior.ClientSetNull)
                 .HasConstraintName("fk_pdt_alb");
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasKey(x => x.IdUtilisateur)
                    .HasName("pk_utl");

                entity.HasOne(d => d.AdresseDeUtilisateur)
                .WithMany(p => p.UtilisateurAdresse)
                .HasForeignKey(d => d.IdAdresse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_utl_adr");
            });

            modelBuilder.Entity<InfoCB>(entity =>
            {
                entity.HasKey(x => x.IdCB)
                    .HasName("pk_icb");

                entity.HasOne(d => d.UtilisateurCB)
                .WithMany(p => p.CBDeUtilisateur)
                .HasForeignKey(d => d.IdCB)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_icb_utl");
            });

            modelBuilder.Entity<Commande>(entity =>
            {
                entity.HasKey(e => new { e.IdCommande })
                   .HasName("pk_cmd");

                entity.HasOne(d => d.UtilisateurCommande)
                    .WithMany(p => p.CommandeDeUtilisateur)
                    .HasForeignKey(d => d.IdUtilisateur)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cmd_utl");
            });


            modelBuilder.Entity<LigneCommande>(entity =>
            {
                entity.HasKey(x => new{ x.IdCommande,x.IdStock})
                    .HasName("pk_lcm");

                entity.HasOne(d => d.CommandeLigne)
                   .WithMany(p => p.LigneDeLaCommande)
                   .HasForeignKey(d => d.IdCommande)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("fk_lcm_cmd");

                entity.HasOne(d => d.StockLigneCommande)
                   .WithMany(p => p.LigneDuStock)
                   .HasForeignKey(d => d.IdStock)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("fk_lcm_stc");

            });


            modelBuilder.Entity<Adresse>(entity =>
            {
                entity.HasKey(x => x.IdAdresse)
                    .HasName("pk_adr");

                entity.HasOne(d => d.VilleAdresse)
              .WithMany(p => p.AdresseDeLaVille)
              .HasForeignKey(d => d.IdVille)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("fk_adr_vil");
            });

            modelBuilder.Entity<Ville>(entity =>
            {
                entity.HasKey(x => x.IdVille)
                    .HasName("pk_vil");

                entity.HasOne(d => d.PaysVille)
               .WithMany(p => p.VilleDuPays)
               .HasForeignKey(d => d.IdPays)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("fk_vil_pay");
            });

            modelBuilder.Entity<Pays>(entity =>
            {
                entity.HasKey(x => x.IdPays)
                    .HasName("pk_pay");
            });

            modelBuilder.Entity<Couleur>(entity =>
            {
                entity.HasKey(x => x.Id)
                    .HasName("pk_clr");
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasKey(x => x.IdAlbum)
                    .HasName("pk_alb");
            });

            modelBuilder.Entity<Marque>(entity =>
            {
                entity.HasKey(x => x.IdMarque)
                    .HasName("pk_mrq");
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(x => x.IdMatch)
                    .HasName("pk_mch");

                entity.HasOne(d => d.EquipeEnMatch)
                .WithMany(p => p.MatchEnEquipe)
                .HasForeignKey(d => d.IdEquipe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mch_eqp");
            });

            modelBuilder.Entity<Poste>(entity =>
            {
                entity.HasKey(x => x.Idposte)
                    .HasName("pk_pst");
            });

            modelBuilder.Entity<Taille>(entity =>
            {
                entity.HasKey(x => x.IdTaille)
                    .HasName("pk_tal");
            });

            modelBuilder.Entity<Equipe>(entity =>
            {
                entity.HasKey(x => x.IdEquipe)
                    .HasName("pk_eqp");

                entity.HasOne(x => x.PaysDeEquipe)
                   .WithMany(x => x.EquipeDuPays)
                   .HasForeignKey(x => x.IdPays)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_eqp_pay");
            });

            modelBuilder.Entity<CouleurProduit>(entity =>
            {
                entity.HasKey(x => new { x.IdCouleurProduit })
                    .HasName("pk_clp");

                entity.HasOne(x => x.Produit_CouleurProduit)
                    .WithMany(x => x.CouleursProduits)
                    .HasForeignKey(x => x.IdProduit)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_clp_pdt");

                entity.HasOne(x => x.Couleur_CouleurProduit)
                    .WithMany(x => x.CouleurProduits)
                    .HasForeignKey(x => x.IdCouleur)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_clp_clr");
            });

            modelBuilder.Entity<Sponsor>(entity =>
            {
                entity.HasKey(x => new { x.IdMarque, x.IdEquipe })
                    .HasName("pk_spc");

                entity.HasOne(x => x.EquipeSponsorise)
                    .WithMany(x => x.SponsorMarque)
                    .HasForeignKey(x => x.IdEquipe)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_spc_eqp");

                entity.HasOne(x => x.MarqueDuSponsor)
                    .WithMany(x => x.SponsorMarque)
                    .HasForeignKey(x => x.IdMarque)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_spc_mrq");
            });

            modelBuilder.Entity<TypeProduit>(entity =>
            {
                entity.HasKey(x => x.Id)
                    .HasName("pk_tpd");

                entity.HasOne(x => x.SurType)
                    .WithMany(x => x.SousTypes)
                    .HasForeignKey(x => x.IdSurType)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_tpd_tpd");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => new { e.IdStock })
                    .HasName("pk_stc");

                entity.HasOne(d => d.TailleDuProduit)
                    .WithMany(p => p.StockDuProduit)
                    .HasForeignKey(d => d.TailleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_stc_tal");

                entity.HasOne(d => d.ProduitEncouleur)
                    .WithMany(p => p.ProduitStock)
                    .HasForeignKey(d => d.CouleurProduitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_stc_clp");

            });

            OnModelBuilderCreatingPartial(modelBuilder);

        }


        partial void OnModelBuilderCreatingPartial(ModelBuilder modelBuilder);

    }
}
