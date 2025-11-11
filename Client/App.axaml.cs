using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.Views;
using Client.Services;
using Client.ViewModels;
using Client.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Client;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Отключаем конфликтующий DataAnnotations плагин
        var toRemove = BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();
        foreach (var p in toRemove) BindingPlugins.DataValidators.Remove(p);

        var services = new ServiceCollection();

        // HttpClient
        services.AddHttpClient<ApiService>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5000/");
        });
        services.AddSingleton<IApiService>(sp => sp.GetRequiredService<ApiService>());

        // Другие сервисы
        services.AddSingleton<ITokenStorage, TokenStorage>();
        services.AddSingleton<INavigationService, NavigationService>();

        // VMs
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<HistoryViewModel>();

        _serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var loginVm = _serviceProvider.GetRequiredService<LoginViewModel>();
            desktop.MainWindow = new LoginWindow { DataContext = loginVm };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static async Task ShowLoginWindowAsync()
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = Current._serviceProvider!.GetRequiredService<LoginViewModel>();
            var win = new LoginWindow { DataContext = vm };
            SwitchWindow(desktop, win);
        }
    }

    public static async Task ShowMainWindowAsync()
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = Current._serviceProvider!.GetRequiredService<MainWindowViewModel>();
            var win = new MainWindow { DataContext = vm };
            SwitchWindow(desktop, win);
        }
    }

    public static async Task ShowHistoryWindowAsync()
    {
        if (Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = Current._serviceProvider!.GetRequiredService<HistoryViewModel>();
            var win = new HistoryWindow { DataContext = vm };
            SwitchWindow(desktop, win);
        }
    }

    private static void SwitchWindow(IClassicDesktopStyleApplicationLifetime desktop, Window newWindow)
    {
        var old = desktop.MainWindow;
        desktop.MainWindow = newWindow;
        newWindow.Show();
        old?.Close();
    }
}