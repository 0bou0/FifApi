using Microsoft.EntityFrameworkCore;

namespace FifApi.Models.EntityFramework
{
    public partial class FifaDBContext : DbContext
    {
        public FifaDBContext() { }
        public FifaDBContext(DbContextOptions<FifaDBContext> options)
            : base(options) { }

        public virtual DbSet<Produit> Produits { get; set; } = null!;
        public virtual DbSet<Couleur> Couleurs { get; set; } = null!;
        public virtual DbSet<CouleurProduit> CouleurProduits { get; set; } = null!;
        public virtual DbSet<TypeProduit> TypeProduits { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produit>(entity =>
            {
                entity.HasKey(x => x.Id)
                    .HasName("pk_pdt");
            });

            modelBuilder.Entity<Couleur>(entity =>
            {
                entity.HasKey(x => x.Id)
                    .HasName("pk_clr");
            });

            modelBuilder.Entity<CouleurProduit>(entity =>
            {
                entity.HasKey(x => new { x.IdProduit, x.IdCouleur })
                    .HasName("pk_clp");

                entity.HasOne(x => x.Produit_CouleurProduit)
                    .WithMany(x => x.CouleursProduit)
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

            OnModelBuilderCreatingPartial(modelBuilder);

        }


        partial void OnModelBuilderCreatingPartial(ModelBuilder modelBuilder);

    }
}
