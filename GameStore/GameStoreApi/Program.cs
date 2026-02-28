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

// 1. Register DbContext 
builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//AutoMapper Profile
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
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

// 2. Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enables the middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enables the Swagger UI (the visual interface)
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();