using Microsoft.EntityFrameworkCore;
using GameStore.Domain.Entities;

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

            // --- SEED DATA ---

            // Predefined Genre IDs
            var strategyId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var sportsId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var racesId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var actionId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = strategyId, Name = "Strategy" },
                new Genre { Id = Guid.NewGuid(), Name = "RTS", ParentGenreId = strategyId },
                new Genre { Id = Guid.NewGuid(), Name = "TBS", ParentGenreId = strategyId },
                new Genre { Id = Guid.NewGuid(), Name = "RPG" },
                new Genre { Id = sportsId, Name = "Sports" },
                new Genre { Id = racesId, Name = "Races" },
                new Genre { Id = Guid.NewGuid(), Name = "Rally", ParentGenreId = racesId },
                new Genre { Id = Guid.NewGuid(), Name = "Arcade", ParentGenreId = racesId },
                new Genre { Id = Guid.NewGuid(), Name = "Formula", ParentGenreId = racesId },
                new Genre { Id = Guid.NewGuid(), Name = "Off-road", ParentGenreId = racesId },
                new Genre { Id = actionId, Name = "Action" },
                new Genre { Id = Guid.NewGuid(), Name = "FPS", ParentGenreId = actionId },
                new Genre { Id = Guid.NewGuid(), Name = "TPS", ParentGenreId = actionId },
                new Genre { Id = Guid.NewGuid(), Name = "Adventure" },
                new Genre { Id = Guid.NewGuid(), Name = "Puzzle & Skill" }
            );

            // Predefined Platform IDs
            modelBuilder.Entity<Platform>().HasData(
                new Platform { Id = Guid.NewGuid(), Type = "Mobile" },
                new Platform { Id = Guid.NewGuid(), Type = "Browser" },
                new Platform { Id = Guid.NewGuid(), Type = "Desktop" },
                new Platform { Id = Guid.NewGuid(), Type = "Console" }
            );
        }
    }
}