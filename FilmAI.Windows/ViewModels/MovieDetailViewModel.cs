using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilmAI.Windows.Models;
using FilmAI.Windows.Services;

namespace FilmAI.Windows.ViewModels;

public partial class MovieDetailViewModel : ObservableObject
{
    private readonly IMovieService _movieService;

    [ObservableProperty]
    private Movie? movie;

    [ObservableProperty]
    private bool isLoading;

    public ObservableCollection<Movie> SimilarMovies { get; } = new();

    public MovieDetailViewModel(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [RelayCommand]
    public async Task LoadAsync(int movieId)
    {
        IsLoading = true;
        try
        {
            Movie = await _movieService.GetMovieDetailsAsync(movieId);

            SimilarMovies.Clear();
            if (Movie is not null)
            {
                // "Bunu sevdiysen şunu da sev" — ortak tür sayısına göre, AI'sız.
                foreach (var similar in await _movieService.GetSimilarMoviesAsync(movieId))
                    SimilarMovies.Add(similar);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ToggleWatchlistAsync()
    {
        if (Movie is null) return;
        await _movieService.ToggleWatchlistAsync(Movie.Id);
        Movie.IsInWatchlist = !Movie.IsInWatchlist;
        OnPropertyChanged(nameof(Movie));
    }

    [RelayCommand]
    private async Task RateAsync(int rating)
    {
        if (Movie is null) return;
        await _movieService.SetUserRatingAsync(Movie.Id, rating);
        Movie.UserRating = rating;
        Movie.IsWatched = true;
        OnPropertyChanged(nameof(Movie));
    }
}
