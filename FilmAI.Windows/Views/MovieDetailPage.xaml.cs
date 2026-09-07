using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using FilmAI.Windows.ViewModels;

namespace FilmAI.Windows.Views;

public sealed partial class MovieDetailPage : Page
{
    public MovieDetailViewModel ViewModel { get; }

    public MovieDetailPage()
    {
        ViewModel = (MovieDetailViewModel)App.Services.GetService(typeof(MovieDetailViewModel))!;
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is int movieId)
            await ViewModel.LoadAsync(movieId);
    }
}
