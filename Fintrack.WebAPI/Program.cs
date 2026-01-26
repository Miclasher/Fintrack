using Fintrack.Domain.Repositories;
using Fintrack.ExternalClients.Abstractions;
using Fintrack.Infrastructure;
using Fintrack.Infrastructure.ExternalClients.Monobank;
using Fintrack.Infrastructure.Repositories;
using Fintrack.Services;
using Fintrack.Services.Abstractions;
using Fintrack.Services.Utilities;
using Fintrack.WebAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Fintrack.WebAPI;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddUserSecrets(typeof(Program).Assembly).Build();

        builder.Services.AddHttpClient("MonobankAPI",
            (sp, client)
            => client.BaseAddress = new Uri("https://api.monobank.ua/"));

        // Add services to the container.
        builder.Services.AddDbContext<FintrackContext>(options
            => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddTransient<ExceptionHandlingMiddleware>();

        builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
        builder.Services.AddScoped<IMonobankClient, MonobankClient>();

        builder.Services.AddScoped<IJwtUtility, JwtUtility>();

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
        builder.Services.AddScoped<IFinancialOperationService, FinancialOperationService>();
        builder.Services.AddScoped<IMonobankImportService, MonobankImportService>();
        builder.Services.AddScoped<ISummaryService, SummaryService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                []
            }
        };

            o.AddSecurityRequirement(securityRequirement);
        });

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
