using gestion_commande.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace gestion_commande.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Livreur> Livreurs { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<ProduitCommande> ProduitsCommandes { get; set; }
      
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Client)
                .WithOne(c => c.User)
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

           modelBuilder.Entity<Client>()
                .HasOne(c => c.User) 
                .WithOne(u => u.Client)
                .HasForeignKey<Client>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            modelBuilder.Entity<Commande>()
                .HasOne(d => d.Client)
                .WithMany(c => c.Commandes)
                .HasForeignKey(d => d.ClientId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Commande>()
                .HasMany(d => d.Details)
                .WithOne(de => de.Commande)
                .HasForeignKey(de => de.CommandeId) 
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Commande>()
                .HasMany(d => d.Paiements)
                .WithOne(p => p.Commande)
                .HasForeignKey(p => p.CommandeId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Produit>()
                .HasMany(a => a.Details)
                .WithOne(de => de.Produit)
                .HasForeignKey(de => de.ProduitId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProduitCommande>()
                .HasKey(e => new {
                    e.ProduitId,
                    e.CommandeId
                });

            modelBuilder.Entity<ProduitCommande>()
                .HasOne(ad => ad.Produit)
                .WithMany(a => a.ProduitsCommande)
                .HasForeignKey(ad => ad.ProduitId);  // Associer la clé étrangère à la colonne ProduitId dans la table ProduitCommandes

            modelBuilder.Entity<ProduitCommande>()
                .HasOne(ad => ad.Commande)
                .WithMany(a => a.ProduitsCommande)
                .HasForeignKey(ad => ad.CommandeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
