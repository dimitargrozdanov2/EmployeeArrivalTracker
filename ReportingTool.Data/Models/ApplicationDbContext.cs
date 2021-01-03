using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ReportingTool.Data.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Arrival> Arrivals { get; set; }
        public virtual DbSet<ServiceToken> ServiceTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Arrival>(entity =>
            {
                entity.ToTable("Arrival");

                entity.Property(e => e.When)
                    .IsRequired()
                    .HasMaxLength(33)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ServiceToken>(entity =>
            {
                entity.HasKey(e => new { e.Token, e.Expires })
                    .HasName("PK__ServiceT__8B52C7A43E823314");

                entity.ToTable("ServiceToken");

                entity.Property(e => e.Token)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Expires)
                    .HasMaxLength(33)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
