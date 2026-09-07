using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using FilmAI.Windows.Services;
using FilmAI.Windows.ViewModels;

namespace FilmAI.Windows;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    public static Window? MainAppWindow { get; private set; }

    public App()
    {
        InitializeComponent();
        Services = ConfigureServices();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainAppWindow = new MainWindow();
        MainAppWindow.Activate();
    }

    /// <summary>
    /// DI konteyneri. Bilinçli olarak IAIService / ClaudeService / GeminiService
    /// KAYDEDİLMEZ — uygulama AI'sız çalışır.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Servisler
        services.AddSingleton<IMovieService, MockMovieService>();
        services.AddSingleton<INavigationService, NavigationService>();

        // ViewModel'ler
        services.AddSingleton<MainViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<DiscoverViewModel>();
        services.AddTransient<MovieDetailViewModel>();
        services.AddTransient<WatchlistViewModel>();

        return services.BuildServiceProvider();
    }
}
