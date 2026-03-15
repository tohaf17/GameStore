using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace GameStore.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var strategyId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var sportsId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var racesId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var actionId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = strategyId, Name = "Strategy" },
                new Genre { Id = Guid.Parse("a51e868b-5777-4509-9430-67180436d40d"), Name = "RTS", ParentGenreId = strategyId },
                new Genre { Id = Guid.Parse("b72e868b-5777-4509-9430-67180436d40e"), Name = "TBS", ParentGenreId = strategyId },
                new Genre { Id = Guid.Parse("c93e868b-5777-4509-9430-67180436d40f"), Name = "RPG" },
                new Genre { Id = sportsId, Name = "Sports" },
                new Genre { Id = racesId, Name = "Races" },
                new Genre { Id = Guid.Parse("d14e868b-5777-4509-9430-67180436d410"), Name = "Rally", ParentGenreId = racesId },
                new Genre { Id = Guid.Parse("e25e868b-5777-4509-9430-67180436d411"), Name = "Arcade", ParentGenreId = racesId },
                new Genre { Id = Guid.Parse("f36e868b-5777-4509-9430-67180436d412"), Name = "Formula", ParentGenreId = racesId },
                new Genre { Id = Guid.Parse("047e868b-5777-4509-9430-67180436d413"), Name = "Off-road", ParentGenreId = racesId },
                new Genre { Id = actionId, Name = "Action" },
                new Genre { Id = Guid.Parse("158e868b-5777-4509-9430-67180436d414"), Name = "FPS", ParentGenreId = actionId },
                new Genre { Id = Guid.Parse("269e868b-5777-4509-9430-67180436d415"), Name = "TPS", ParentGenreId = actionId },
                new Genre { Id = Guid.Parse("37ae868b-5777-4509-9430-67180436d416"), Name = "Adventure" },
                new Genre { Id = Guid.Parse("48be868b-5777-4509-9430-67180436d417"), Name = "Puzzle & Skill" }
            );

            // Фіксовані ID для Платформ
            modelBuilder.Entity<Platform>().HasData(
                new Platform { Id = Guid.Parse("96a6669c-f947-4183-9602-047b198d0295"), Type = "Mobile" },
                new Platform { Id = Guid.Parse("72c21960-9173-4424-916c-e09355745772"), Type = "Browser" },
                new Platform { Id = Guid.Parse("6d07e997-8c46-4e5a-939e-29249767675f"), Type = "Desktop" },
                new Platform { Id = Guid.Parse("af6d6e6f-537a-4933-8711-209a3930b207"), Type = "Console" }
            );
        }
    }
}
