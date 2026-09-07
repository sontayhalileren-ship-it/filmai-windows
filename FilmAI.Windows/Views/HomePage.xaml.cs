using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FilmAI.Windows.Controls;
using FilmAI.Windows.Models;
using FilmAI.Windows.Services;
using FilmAI.Windows.ViewModels;

namespace FilmAI.Windows.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel { get; }

    public HomePage()
    {
        ViewModel = (HomeViewModel)App.Services.GetService(typeof(HomeViewModel))!;
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

    private void OnTonightsPickClicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.TonightsPick is Movie pick)
        {
            var navService = (INavigationService)App.Services.GetService(typeof(INavigationService))!;
            navService.NavigateTo(typeof(MovieDetailPage), pick.Id);
        }
    }
}
