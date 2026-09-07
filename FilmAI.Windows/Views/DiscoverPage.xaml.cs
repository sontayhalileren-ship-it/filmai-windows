using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using FilmAI.Windows.Controls;
using FilmAI.Windows.Services;
using FilmAI.Windows.ViewModels;

namespace FilmAI.Windows.Views;

public sealed partial class DiscoverPage : Page
{
    public DiscoverViewModel ViewModel { get; }

    public DiscoverPage()
    {
        ViewModel = (DiscoverViewModel)App.Services.GetService(typeof(DiscoverViewModel))!;
        InitializeComponent();
    }

    private void OnGenreChecked(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox { Tag: string genre })
            ViewModel.ToggleGenreCommand.Execute(genre);
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
