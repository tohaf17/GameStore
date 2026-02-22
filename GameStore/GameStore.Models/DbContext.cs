using Microsoft.EntityFrameworkCore;
namespace GameStore.Models
{
    public class GameStoreContext:DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options):base(options) { }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasIndex(g=>g.Key).IsUnique();

            modelBuilder.Entity<Genre>()
                .HasIndex(g => g.Name).IsUnique();

            modelBuilder.Entity<Platform>()
                .HasIndex(p => p.Type).IsUnique();

            //Many to Many for Game and Genre
            modelBuilder.Entity<GameGenre>()
                .HasKey(gg => new { gg.GameId, gg.GenreId });

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg=>gg.Game)
                .WithMany(g=>g.GameGenres)
                .HasForeignKey(gg=>gg.GameId);

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Genre)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(gg => gg.GenreId);
            //Many to Many for Game and Platform
            modelBuilder.Entity<GamePlatform>()
                .HasKey(gp => new { gp.GameId, gp.PlatformId });

            modelBuilder.Entity<GamePlatform>()
                .HasOne(gp => gp.Platform)
                .WithMany(g=>g.GamePlatforms)
                .HasForeignKey(gp => gp.PlatformId);

            modelBuilder.Entity<GamePlatform>()
                .HasOne(gp => gp.Game)
                .WithMany(g => g.GamePlatforms)
                .HasForeignKey(gp => gp.GameId);

            modelBuilder.Entity<Genre>()
        .HasOne<Genre>()
        .WithMany()
        .HasForeignKey(g => g.ParentGenreId)
        .OnDelete(DeleteBehavior.Restrict);

            // Predefined IDs so we can reference them in code/requests
            var strategyId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var sportsId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var racesId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var actionId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            modelBuilder.Entity<Genre>().HasData(
                // Strategy & Subgenres
                new Genre { Id = strategyId, Name = "Strategy" },
                new Genre { Id = Guid.NewGuid(), Name = "RTS", ParentGenreId = strategyId },
                new Genre { Id = Guid.NewGuid(), Name = "TBS", ParentGenreId = strategyId },

                // Others
                new Genre { Id = Guid.NewGuid(), Name = "RPG" },
                new Genre { Id = sportsId, Name = "Sports" },

                // Races & Subgenres
                new Genre { Id = racesId, Name = "Races" },
                new Genre { Id = Guid.NewGuid(), Name = "Rally", ParentGenreId = racesId },
                new Genre { Id = Guid.NewGuid(), Name = "Arcade", ParentGenreId = racesId },
                new Genre { Id = Guid.NewGuid(), Name = "Formula", ParentGenreId = racesId },
                new Genre { Id = Guid.NewGuid(), Name = "Off-road", ParentGenreId = racesId },

                // Action & Subgenres
                new Genre { Id = actionId, Name = "Action" },
                new Genre { Id = Guid.NewGuid(), Name = "FPS", ParentGenreId = actionId },
                new Genre { Id = Guid.NewGuid(), Name = "TPS", ParentGenreId = actionId },

                new Genre { Id = Guid.NewGuid(), Name = "Adventure" },
                new Genre { Id = Guid.NewGuid(), Name = "Puzzle & Skill" }
            );

        }


    }
}
