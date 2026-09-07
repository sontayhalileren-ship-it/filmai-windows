using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FilmAI.Windows.Models;

namespace FilmAI.Windows.Services;

/// <summary>
/// Geliştirme aşaması için sahte veri servisi. Gerçek TMDB/IMDb entegrasyonu
/// gelene kadar UI'ı beslemek amacıyla kullanılır. Hiçbir AI çağrısı yapmaz;
/// "hidden gem" ve "tonight's pick" gibi öneriler basit kurallarla hesaplanır.
/// </summary>
public class MockMovieService : IMovieService
{
    private readonly List<Movie> _movies;

    public MockMovieService()
    {
        _movies = BuildSeedData();
    }

    public Task<List<Movie>> SearchMoviesAsync(MovieSearchFilter filter, CancellationToken ct = default)
    {
        IEnumerable<Movie> query = _movies;

        if (filter.Genres.Count > 0)
            query = query.Where(m => m.Genres.Any(g => filter.Genres.Contains(g, StringComparer.OrdinalIgnoreCase)));

        if (filter.MinRating.HasValue)
            query = query.Where(m => m.Rating >= filter.MinRating.Value);

        if (filter.MaxRuntimeMinutes.HasValue)
            query = query.Where(m => m.RuntimeMinutes <= filter.MaxRuntimeMinutes.Value);

        if (filter.ReleaseYearFrom.HasValue)
            query = query.Where(m => m.ReleaseDate.Year >= filter.ReleaseYearFrom.Value);

        if (filter.ReleaseYearTo.HasValue)
            query = query.Where(m => m.ReleaseDate.Year <= filter.ReleaseYearTo.Value);

        if (!string.IsNullOrWhiteSpace(filter.Director))
            query = query.Where(m => m.Director.Contains(filter.Director, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
            query = query.Where(m => m.Title.Contains(filter.SearchText, StringComparison.OrdinalIgnoreCase));

        query = filter.SortOrder switch
        {
            MovieSortOrder.RatingDesc => query.OrderByDescending(m => m.Rating),
            MovieSortOrder.ReleaseDateDesc => query.OrderByDescending(m => m.ReleaseDate),
            MovieSortOrder.TitleAsc => query.OrderBy(m => m.Title),
            _ => query.OrderByDescending(m => m.Rating).ThenByDescending(m => m.ReleaseDate),
        };

        return Task.FromResult(query.ToList());
    }

    public Task<Movie?> GetMovieDetailsAsync(int movieId, CancellationToken ct = default)
        => Task.FromResult(_movies.FirstOrDefault(m => m.Id == movieId));

    public Task<List<Movie>> GetTrendingAsync(CancellationToken ct = default)
        => Task.FromResult(_movies.OrderByDescending(m => m.Rating).Take(10).ToList());

    public Task<List<Movie>> GetHiddenGemsAsync(CancellationToken ct = default)
        => Task.FromResult(_movies.Where(m => m.IsHiddenGem).ToList());

    /// <summary>
    /// AI olmadan, basit kural tabanlı "günün önerisi": kullanıcının sevdiği
    /// türlerden, henüz izlenmemiş, en yüksek puanlı film.
    /// </summary>
    public Task<Movie?> GetTonightsPickAsync(UserPreference preference, CancellationToken ct = default)
    {
        var candidates = _movies.Where(m => !m.IsWatched);

        if (preference.FavoriteGenres.Count > 0)
            candidates = candidates.Where(m => m.Genres.Any(g => preference.FavoriteGenres.Contains(g, StringComparer.OrdinalIgnoreCase)));

        if (preference.ExcludedGenres.Count > 0)
            candidates = candidates.Where(m => !m.Genres.Any(g => preference.ExcludedGenres.Contains(g, StringComparer.OrdinalIgnoreCase)));

        var pick = candidates.Where(m => m.Rating >= preference.MinRating)
                              .OrderByDescending(m => m.Rating)
                              .FirstOrDefault();

        return Task.FromResult(pick);
    }

    /// <summary>
    /// "Benzer filmler": aynı tür(ler)i paylaşan diğer yapımlar, ortak tür
    /// sayısına göre sıralanır. AI kullanılmaz.
    /// </summary>
    public Task<List<Movie>> GetSimilarMoviesAsync(int movieId, CancellationToken ct = default)
    {
        var source = _movies.FirstOrDefault(m => m.Id == movieId);
        if (source is null)
            return Task.FromResult(new List<Movie>());

        var similar = _movies
            .Where(m => m.Id != movieId)
            .Select(m => new { Movie = m, Overlap = m.Genres.Intersect(source.Genres, StringComparer.OrdinalIgnoreCase).Count() })
            .Where(x => x.Overlap > 0)
            .OrderByDescending(x => x.Overlap)
            .ThenByDescending(x => x.Movie.Rating)
            .Take(6)
            .Select(x => x.Movie)
            .ToList();

        return Task.FromResult(similar);
    }

    public Task<List<Movie>> GetWatchlistAsync(CancellationToken ct = default)
        => Task.FromResult(_movies.Where(m => m.IsInWatchlist).ToList());

    public Task ToggleWatchlistAsync(int movieId, CancellationToken ct = default)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == movieId);
        if (movie is not null)
            movie.IsInWatchlist = !movie.IsInWatchlist;

        return Task.CompletedTask;
    }

    public Task SetUserRatingAsync(int movieId, int rating, CancellationToken ct = default)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == movieId);
        if (movie is not null)
        {
            movie.UserRating = rating;
            movie.IsWatched = true;
        }

        return Task.CompletedTask;
    }

    private static List<Movie> BuildSeedData() => new()
    {
        new Movie
        {
            Id = 1, Title = "Arrival", OriginalTitle = "Arrival",
            Overview = "Bir dilbilimci, dünyayı ziyaret eden uzaylılarla iletişim kurmaya çalışır.",
            Rating = 8.0, ReleaseDate = new DateTime(2016, 11, 11), RuntimeMinutes = 116,
            AgeRating = "13+", Genres = new() { "Sci-Fi", "Drama" }, Director = "Denis Villeneuve",
            IsHiddenGem = false,
        },
        new Movie
        {
            Id = 2, Title = "Annihilation", OriginalTitle = "Annihilation",
            Overview = "Bir grup bilim insanı, doğa kurallarının bozulduğu gizemli bir bölgeye girer.",
            Rating = 6.9, ReleaseDate = new DateTime(2018, 2, 23), RuntimeMinutes = 115,
            AgeRating = "18+", Genres = new() { "Sci-Fi", "Horror" }, Director = "Alex Garland",
            IsHiddenGem = true,
        },
        new Movie
        {
            Id = 3, Title = "Blade Runner 2049", OriginalTitle = "Blade Runner 2049",
            Overview = "Yeni nesil bir blade runner, uzun süredir gizli kalmış bir sırrı ortaya çıkarır.",
            Rating = 8.0, ReleaseDate = new DateTime(2017, 10, 6), RuntimeMinutes = 164,
            AgeRating = "16+", Genres = new() { "Sci-Fi", "Thriller" }, Director = "Denis Villeneuve",
            IsHiddenGem = false,
        },
        new Movie
        {
            Id = 4, Title = "The Fall", OriginalTitle = "The Fall",
            Overview = "Hastanede yatan bir kaskadör, küçük bir kıza olağanüstü bir hikâye anlatır.",
            Rating = 7.9, ReleaseDate = new DateTime(2006, 5, 9), RuntimeMinutes = 117,
            AgeRating = "13+", Genres = new() { "Fantasy", "Drama" }, Director = "Tarsem Singh",
            IsHiddenGem = true,
        },
        new Movie
        {
            Id = 5, Title = "Coherence", OriginalTitle = "Coherence",
            Overview = "Bir yemek daveti, bir kuyruklu yıldızın geçişiyle birlikte gerçeküstü bir hal alır.",
            Rating = 7.2, ReleaseDate = new DateTime(2013, 6, 19), RuntimeMinutes = 89,
            AgeRating = "13+", Genres = new() { "Sci-Fi", "Mystery" }, Director = "James Ward Byrkit",
            IsHiddenGem = true,
        },
        new Movie
        {
            Id = 6, Title = "Whiplash", OriginalTitle = "Whiplash",
            Overview = "Genç bir davulcu, acımasız bir müzik öğretmeninin baskısı altında sınırlarını zorlar.",
            Rating = 8.5, ReleaseDate = new DateTime(2014, 10, 10), RuntimeMinutes = 106,
            AgeRating = "16+", Genres = new() { "Drama", "Music" }, Director = "Damien Chazelle",
            IsHiddenGem = false,
        },
        new Movie
        {
            Id = 7, Title = "Prisoners", OriginalTitle = "Prisoners",
            Overview = "Bir babanın, kayıp kızını bulmak için verdiği umutsuz mücadele.",
            Rating = 8.1, ReleaseDate = new DateTime(2013, 9, 20), RuntimeMinutes = 153,
            AgeRating = "16+", Genres = new() { "Thriller", "Drama" }, Director = "Denis Villeneuve",
            IsHiddenGem = false,
        },
        new Movie
        {
            Id = 8, Title = "Under the Skin", OriginalTitle = "Under the Skin",
            Overview = "İnsan kılığına giren bir varlık, Glasgow sokaklarında av peşindedir.",
            Rating = 6.3, ReleaseDate = new DateTime(2013, 11, 14), RuntimeMinutes = 108,
            AgeRating = "18+", Genres = new() { "Sci-Fi", "Horror" }, Director = "Jonathan Glazer",
            IsHiddenGem = true,
        },
    };
}
