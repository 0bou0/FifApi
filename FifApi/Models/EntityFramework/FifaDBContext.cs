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
                    .HasName("pk_alb");
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(x => x.IdMatch)
                    .HasName("pk_mch");
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
                entity.HasKey(e => new { e.TailleId, e.CouleurProduitId })
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
