using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "ParentGenreId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Strategy", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Sports", null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Races", null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Action", null },
                    { new Guid("60e139bc-13c1-452c-bf7e-0972ab15e87c"), "RPG", null },
                    { new Guid("98c63308-ed17-491a-8e50-e84341a947fa"), "Puzzle & Skill", null },
                    { new Guid("e95c0088-1b19-449a-850b-2a7a6134f371"), "Adventure", null },
                    { new Guid("96bef387-65b9-4c43-a79f-b457e0f8c519"), "TBS", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b915d110-3dd0-4021-98fb-622a8241d8b6"), "Off-road", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("bcb92b0b-dcb3-4adf-877c-c5ae68ed79e1"), "Formula", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("c0770557-baf9-473b-b58c-f2f0368946d0"), "TPS", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("ce4016d5-2e18-4cf2-8d8d-e345646b0384"), "Arcade", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("d5c016b8-1bc9-4506-acb2-d578f9fa2f2a"), "Rally", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("ef9ec81e-e87b-43b5-a905-ca197a0c30f8"), "FPS", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("fa7ded2e-fd24-4c80-b0fa-8180da1a09d0"), "RTS", new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ParentGenreId",
                table: "Genres",
                column: "ParentGenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Genres_ParentGenreId",
                table: "Genres",
                column: "ParentGenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Genres_ParentGenreId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_ParentGenreId",
                table: "Genres");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("60e139bc-13c1-452c-bf7e-0972ab15e87c"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("96bef387-65b9-4c43-a79f-b457e0f8c519"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("98c63308-ed17-491a-8e50-e84341a947fa"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b915d110-3dd0-4021-98fb-622a8241d8b6"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("bcb92b0b-dcb3-4adf-877c-c5ae68ed79e1"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("c0770557-baf9-473b-b58c-f2f0368946d0"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ce4016d5-2e18-4cf2-8d8d-e345646b0384"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("d5c016b8-1bc9-4506-acb2-d578f9fa2f2a"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("e95c0088-1b19-449a-850b-2a7a6134f371"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ef9ec81e-e87b-43b5-a905-ca197a0c30f8"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("fa7ded2e-fd24-4c80-b0fa-8180da1a09d0"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
        }
    }
}
