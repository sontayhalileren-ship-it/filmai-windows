using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FilmAI.Windows.Models;

namespace FilmAI.Windows.Services;

/// <summary>
/// Film verisi erişim katmanı. Bilinçli olarak hiçbir AI/LLM metodu içermez
/// (ör. AskAssistant, GetAiRecommendations vb. YOK).
/// </summary>
public interface IMovieService
{
    Task<List<Movie>> SearchMoviesAsync(MovieSearchFilter filter, CancellationToken ct = default);
    Task<Movie?> GetMovieDetailsAsync(int movieId, CancellationToken ct = default);
    Task<List<Movie>> GetTrendingAsync(CancellationToken ct = default);
    Task<List<Movie>> GetHiddenGemsAsync(CancellationToken ct = default);
    Task<Movie?> GetTonightsPickAsync(UserPreference preference, CancellationToken ct = default);
    Task<List<Movie>> GetSimilarMoviesAsync(int movieId, CancellationToken ct = default);
    Task<List<Movie>> GetWatchlistAsync(CancellationToken ct = default);
    Task ToggleWatchlistAsync(int movieId, CancellationToken ct = default);
    Task SetUserRatingAsync(int movieId, int rating, CancellationToken ct = default);
}
