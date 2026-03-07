using AutoMapper;
using GameStore.Application;
using GameStore.Infrastructure.Data;
using GameStore.Repositories.Interfaces;
using GameStore.Repositories.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStoreApi;

var builder = WebApplication.CreateBuilder(args);
//DbContext
builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Mapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
//Controllers
builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default1Min",
        new CacheProfile()
        {
            Duration = 60,
            Location = ResponseCacheLocation.Any
        });
});
//Unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//Register game service and repository
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

//Register platform service and repository
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<IPlatformService, PlatformService>();

//Register genre service and repository
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();