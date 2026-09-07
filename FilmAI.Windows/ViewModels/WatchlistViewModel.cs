using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilmAI.Windows.Models;
using FilmAI.Windows.Services;

namespace FilmAI.Windows.ViewModels;

public partial class WatchlistViewModel : ObservableObject
{
    private readonly IMovieService _movieService;

    [ObservableProperty]
    private bool isLoading;

    public ObservableCollection<Movie> Items { get; } = new();

    public WatchlistViewModel(IMovieService movieService)
    {
        _movieService = movieService;
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsLoading = true;
        try
        {
            Items.Clear();
            foreach (var movie in await _movieService.GetWatchlistAsync())
                Items.Add(movie);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RemoveAsync(Movie movie)
    {
        await _movieService.ToggleWatchlistAsync(movie.Id);
        Items.Remove(movie);
    }
}
