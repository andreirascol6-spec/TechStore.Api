using Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Windows;
using TechStore.Wpf.Connection;
using TechStore.Wpf.ViewModels;

namespace TechStore.Wpf;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        Services = serviceCollection.BuildServiceProvider();

        // 1. Schimbăm aici: cerem LoginWindow din container în loc de MainWindow
        var loginWindow = Services.GetRequiredService<LoginWindow>();
        loginWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Înregistrăm ApiService și îi configurăm adresa de bază
        services.AddSingleton<ApiService>(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7135/api/") // Adresa API-ului tău
            };
            return new ApiService(httpClient);
        });

        // ViewModel-uri
        services.AddTransient<ProdusViewModel>();
        services.AddTransient<ComandaViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<LoginViewModel>();

        // Ferestre
        services.AddTransient<MainWindow>();
        services.AddTransient<LoginWindow>();
    }
} 