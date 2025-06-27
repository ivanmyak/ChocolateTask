// File: ChocolateAPI/Program.cs
using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using HotChocolate.AspNetCore.Authorization;
using ChocolateAPI.Data;
using ChocolateAPI.Entites;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ChocolateAPI.GraphQL;
using HotChocolate.Types.Pagination;

Env.Load();  // загрузить .env в переменные окружения

var builder = WebApplication.CreateBuilder(args);

// 1) Загрузка конфигурации
builder.Configuration
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
               optional: true, reloadOnChange: true)
  .AddEnvironmentVariables()             // .env и системные переменные
  .AddKeyPerFile("/run/secrets", true);  // Docker Secrets

// 2) Проверка обязательных ключей
var required = new[]
{
  "ASPNETCORE_ENVIRONMENT",
  "ChocolateConnectionString",
  "JwtSettings:Issuer",
  "JwtSettings:Audience",
  "JwtSettings:SecretKey"
};
var missing = required
  .Where(k => string.IsNullOrWhiteSpace(builder.Configuration[k]))
  .ToList();
if (missing.Any())
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: Missing required configuration:");
    missing.ForEach(k => Console.WriteLine($"  - {k}"));
    Console.ResetColor();
    return;
}

// 3) JWT-настройки
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);
var jwt = jwtSection.Get<JwtSettings>()!;

// 4) Аутентификация через JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opt =>
  {
      opt.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = jwt.Issuer,
          ValidateAudience = true,
          ValidAudience = jwt.Audience,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(
                                     Encoding.UTF8.GetBytes(jwt.SecretKey))
      };
  });

// 5) EF Core + PostgreSQL
builder.Services.AddPooledDbContextFactory<ChocolateDbContext>(opt =>
  opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// 6) GraphQL HotChocolate
builder.Services
  .AddGraphQLServer()
  .AddAuthorization()       // разрешить [Authorize] в запросах
  .AddQueryType<ChocolateAPI.GraphQL.Query>()
  .AddMutationType<Mutation>()
  .AddFiltering()
  .AddSorting()
  .AddProjections();



var app = builder.Build();

// 7) Middleware
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL();  // endpoint: /graphql

app.Run();
