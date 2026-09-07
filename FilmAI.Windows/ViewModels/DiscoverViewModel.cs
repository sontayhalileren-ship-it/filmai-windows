using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FilmAI.Windows.Models;
using FilmAI.Windows.Services;

namespace FilmAI.Windows.ViewModels;

/// <summary>
/// Klasik filtre paneli: tür, min puan, süre, yıl aralığı, yönetmen, sıralama.
/// Doğal dil / AI tabanlı arama YOKTUR; tüm filtreleme UI kontrolleri üzerinden yapılır.
/// </summary>
public partial class DiscoverViewModel : ObservableObject
{
    private readonly IMovieService _movieService;

    [ObservableProperty]
    private string? searchText;

    [ObservableProperty]
    private double minRating = 0;

    [ObservableProperty]
    private int? releaseYearFrom;

    [ObservableProperty]
    private int? releaseYearTo;

    [ObservableProperty]
    private string? director;

    [ObservableProperty]
    private MovieSortOrder sortOrder = MovieSortOrder.PopularityDesc;

    [ObservableProperty]
    private bool isLoading;

    public ObservableCollection<string> AvailableGenres { get; } = new()
    {
        "Sci-Fi", "Drama", "Thriller", "Horror", "Mystery", "Fantasy", "Music"
    };

    public ObservableCollection<string> SelectedGenres { get; } = new();
    public ObservableCollection<Movie> Results { get; } = new();

    public DiscoverViewModel(IMovieService movieService)
    {
        _movieService = movieService;
        _ = SearchAsync();
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        IsLoading = true;
        try
        {
            var filter = new MovieSearchFilter
            {
                Genres = new(SelectedGenres),
                MinRating = MinRating > 0 ? MinRating : null,
                ReleaseYearFrom = ReleaseYearFrom,
                ReleaseYearTo = ReleaseYearTo,
                Director = Director,
                SearchText = SearchText,
                SortOrder = SortOrder,
            };

            var movies = await _movieService.SearchMoviesAsync(filter);

            Results.Clear();
            foreach (var movie in movies)
                Results.Add(movie);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void ToggleGenre(string genre)
    {
        if (SelectedGenres.Contains(genre))
            SelectedGenres.Remove(genre);
        else
            SelectedGenres.Add(genre);

        _ = SearchAsync();
    }

    [RelayCommand]
    private void ClearFilters()
    {
        SelectedGenres.Clear();
        MinRating = 0;
        ReleaseYearFrom = null;
        ReleaseYearTo = null;
        Director = null;
        SearchText = null;
        SortOrder = MovieSortOrder.PopularityDesc;
        _ = SearchAsync();
    }
}
