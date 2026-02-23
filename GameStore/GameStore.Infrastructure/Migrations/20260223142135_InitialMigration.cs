using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentGenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genres_Genres_ParentGenreId",
                        column: x => x.ParentGenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => new { x.GameId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GameGenre_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatform", x => new { x.GameId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_GamePlatform_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatform_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "ParentGenreId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Strategy", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Sports", null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Races", null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Action", null },
                    { new Guid("b17c9c4c-840b-4d01-afa1-034bb2e31d42"), "Puzzle & Skill", null },
                    { new Guid("d7fd33ca-a352-4066-88ef-bac690ac6988"), "Adventure", null },
                    { new Guid("e5e9b9a3-4197-4803-8be2-ef3a99edf563"), "RPG", null }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { new Guid("87ddb679-74d4-4730-a2b1-d50c81e153a5"), "Console" },
                    { new Guid("9a093aae-88f8-4c6e-877e-7def4be39885"), "Browser" },
                    { new Guid("a3869ef6-f8a1-4274-af5c-043d915ca431"), "Desktop" },
                    { new Guid("f43081ff-50c9-422d-a763-629e4e6c334f"), "Mobile" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "ParentGenreId" },
                values: new object[,]
                {
                    { new Guid("1b3930e8-798c-4aa5-9411-ccdbe5f22157"), "TBS", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("397f89a5-adc9-48d3-b39b-e07e31c8c0bd"), "Formula", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("4664165d-134e-45f6-8529-449079c535f6"), "TPS", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("46e429b3-a710-496b-b209-8d14aebe69ae"), "FPS", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("5e4f6b10-1792-4c29-b9d5-25b3ea4b3689"), "Arcade", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("866e4138-e354-45b3-9a62-c48edc9a983b"), "RTS", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("8798beee-97ef-48f1-bc63-3056d1634687"), "Rally", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("b7db66eb-e58c-4dfb-91a6-bf2413ed43e2"), "Off-road", new Guid("33333333-3333-3333-3333-333333333333") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GenreId",
                table: "GameGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatform_PlatformId",
                table: "GamePlatform",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Key",
                table: "Games",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ParentGenreId",
                table: "Genres",
                column: "ParentGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Platforms_Type",
                table: "Platforms",
                column: "Type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "GamePlatform");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}
