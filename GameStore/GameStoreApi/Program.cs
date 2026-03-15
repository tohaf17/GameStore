using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using GameStore.Application;
using GameStore.Application.DTO;
using GameStore.Infrastructure.Data;
using GameStore.Repositories.Interfaces;
using GameStore.Repositories.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Services.Services;
using GameStoreApi.Validation;
using GameStoreApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//ExceptionMapper
builder.Services.AddExceptionMappers();

//ProblemDetails
builder.Services.AddProblemDetails();

//ValidationFilter
builder.Services.AddScoped<ValidationFilter>();

//Async sufix + Validators + Controllers + CacheProfiles
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
    options.Filters.Add<ValidationFilter>();
    options.CacheProfiles.Add("Default1Min",
        new CacheProfile()
        {
            Duration = 60,
            Location = ResponseCacheLocation.Any
        });
});

//Validators
builder.Services.AddValidatorsFromAssemblyContaining<GameValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//DbContext
builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Mapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

//Unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//game service and repository
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

//platform service and repository
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<IPlatformService, PlatformService>();

//genre service and repository
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseResponseCaching();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();