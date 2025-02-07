using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
// using FluentValidation.AspNetCore;
using FluentValidation;
using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Repositories;
using api.Services;
using api.Validators;

// Create a new instance of the WebApplication builder.
var builder = WebApplication.CreateBuilder(args);

// Add files or variables to the configuration.
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
    //   .AddEnvironmentVariables();

// Configure database context.
builder.Services
    .AddDbContext<AppDbContext>(options =>
    {
        // Use SQL Server as the database provider.
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

// Configure identity services.
builder.Services
    .AddIdentity<User, IdentityRole>()
    // Use Entity Framework Core to store identity data.
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register services and repositories as transient services.
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IConfigGenderRepository, ConfigGenderRepository>();
builder.Services.AddTransient<IConfigGenderService, ConfigGenderService>();

// Add controllers to the container and configure JSON serialization options.
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => 
    {
        // Ignore reference loops when serializing objects.
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    })
    // Register FluentValidation
    // .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserValidator>())
    ;

builder.Services.AddTransient<IValidator<RequestUserDTO>, RequestUserDTOValidator>();
builder.Services.AddTransient<IValidator<RequestConfigGenderDTO>, RequestConfigGenderDTOValidator>();

// Configure CORS policy.
builder.Services 
    .AddCors(options =>
    {
        // Create a new CORS policy.
        options.AddPolicy("SameOriginOnly", policy =>
        {
            policy
                .AllowCredentials()
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader();
        });
    });

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Demo API");
    });
}

// app.UseHttpsRedirection();
// Use CORS policy.
app.UseCors("SameOriginOnly");
app.UseRouting();
app.MapControllers();
app.Run();