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
        public virtual DbSet<CouleurProduit> CouleurProduit { get; set; } = null!;


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
                    .WithMany(x => x.CouleursProduit)
                    .HasForeignKey(x => x.IdCouleur)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_clp_clr");
            });

            OnModelBuilderCreatingPartial(modelBuilder);

        }

        partial void OnModelBuilderCreatingPartial(ModelBuilder modelBuilder);

    }
}
