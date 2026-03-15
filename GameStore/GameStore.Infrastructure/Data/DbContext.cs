using Microsoft.EntityFrameworkCore;
using GameStore.Domain.Entities;
using System;

namespace GameStore.Infrastructure.Data
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options) { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Unique Constraints
            modelBuilder.Entity<Game>().HasIndex(g => g.Key).IsUnique();
            modelBuilder.Entity<Genre>().HasIndex(g => g.Name).IsUnique();
            modelBuilder.Entity<Platform>().HasIndex(p => p.Type).IsUnique();

            // 2. Many-to-Many: Game <-> Genre
            modelBuilder.Entity<GameGenre>().HasKey(gg => new { gg.GameId, gg.GenreId });
            modelBuilder.Entity<GameGenre>().HasOne(gg => gg.Game).WithMany(g => g.GameGenres).HasForeignKey(gg => gg.GameId);
            modelBuilder.Entity<GameGenre>().HasOne(gg => gg.Genre).WithMany(g => g.GameGenres).HasForeignKey(gg => gg.GenreId);

            // 3. Many-to-Many: Game <-> Platform
            modelBuilder.Entity<GamePlatform>().HasKey(gp => new { gp.GameId, gp.PlatformId });
            modelBuilder.Entity<GamePlatform>().HasOne(gp => gp.Platform).WithMany(p => p.GamePlatforms).HasForeignKey(gp => gp.PlatformId);
            modelBuilder.Entity<GamePlatform>().HasOne(gp => gp.Game).WithMany(g => g.GamePlatforms).HasForeignKey(gp => gp.GameId);

            // 4. Genre Hierarchy (Subgenres)
            modelBuilder.Entity<Genre>()
                .HasOne<Genre>()
                .WithMany()
                .HasForeignKey(g => g.ParentGenreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Seed();

        }
    }
}