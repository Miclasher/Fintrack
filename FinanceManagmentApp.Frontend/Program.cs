using FinanceManagmentApp.Frontend.Services;
using FinanceManagmentApp.Frontend.Services.Abstractions;
using FinanceManagmentApp.Frontend.Utilities;
using Fintrack.Frontend.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinanceManagmentApp.Frontend;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddHttpClient("FinanceManagmentAppAPI",
            (sp, client)
            => client.BaseAddress = new Uri(sp.GetRequiredService<IConfiguration>()["ApiBaseUrl"]!));

        builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITransactionTypeService, TransactionTypeService>();
        builder.Services.AddScoped<IFinancialOperationService, FinancialOperationService>();
        builder.Services.AddScoped<ISummaryService, SummaryService>();
        builder.Services.AddScoped<IMonobankImportService, MonobankImportService>();

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.MapStaticAssets();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.UseAuthorization();

        app.Run();
    }
}
