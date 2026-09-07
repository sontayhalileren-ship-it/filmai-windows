using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilmAI.Windows.Models;
using FilmAI.Windows.Services;

namespace FilmAI.Windows.ViewModels;

/// <summary>
/// Anasayfa view model'i. "Günün Önerisi" burada AI ile değil, kullanıcının
/// kayıtlı tür tercihine göre basit bir kuralla (bkz. MockMovieService.GetTonightsPickAsync)
/// hesaplanır.
/// </summary>
public partial class HomeViewModel : ObservableObject
{
    private readonly IMovieService _movieService;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private Movie? tonightsPick;

    public ObservableCollection<Movie> Trending { get; } = new();
    public ObservableCollection<Movie> HiddenGems { get; } = new();

    public HomeViewModel(IMovieService movieService)
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
            var preference = new UserPreference
            {
                FavoriteGenres = new() { "Sci-Fi", "Drama" },
                MinRating = 7.0,
            };

            TonightsPick = await _movieService.GetTonightsPickAsync(preference);

            Trending.Clear();
            foreach (var movie in await _movieService.GetTrendingAsync())
                Trending.Add(movie);

            HiddenGems.Clear();
            foreach (var movie in await _movieService.GetHiddenGemsAsync())
                HiddenGems.Add(movie);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
