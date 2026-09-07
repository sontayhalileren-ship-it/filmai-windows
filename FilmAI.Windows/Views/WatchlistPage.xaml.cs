using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FilmAI.Windows.Controls;
using FilmAI.Windows.Services;
using FilmAI.Windows.ViewModels;

namespace FilmAI.Windows.Views;

public sealed partial class WatchlistPage : Page
{
    public WatchlistViewModel ViewModel { get; }

    public WatchlistPage()
    {
        ViewModel = (WatchlistViewModel)App.Services.GetService(typeof(WatchlistViewModel))!;
        InitializeComponent();
    }

    private void OnMovieCardClicked(object sender, RoutedEventArgs e)
    {
        if (sender is MovieCard card)
        {
            var navService = (INavigationService)App.Services.GetService(typeof(INavigationService))!;
            navService.NavigateTo(typeof(MovieDetailPage), card.Movie.Id);
        }
    }
}
