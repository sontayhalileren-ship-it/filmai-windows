using System.Collections.Generic;

namespace FilmAI.Windows.Models;

/// <summary>
/// Kullanıcının filtre bazlı zevk profili. AI yok; öneriler burada tanımlı
/// basit kurallara göre (sevilen türler / min puan / yönetmen ağırlığı) hesaplanır.
/// </summary>
public class UserPreference
{
    public List<string> FavoriteGenres { get; set; } = new();
    public List<string> ExcludedGenres { get; set; } = new();
    public double MinRating { get; set; } = 6.5;
    public List<string> FavoriteDirectors { get; set; } = new();
}

public class MovieSearchFilter
{
    public List<string> Genres { get; set; } = new();
    public double? MinRating { get; set; }
    public int? MaxRuntimeMinutes { get; set; }
    public int? ReleaseYearFrom { get; set; }
    public int? ReleaseYearTo { get; set; }
    public string? Director { get; set; }
    public string? SearchText { get; set; }
    public MovieSortOrder SortOrder { get; set; } = MovieSortOrder.PopularityDesc;
}

public enum MovieSortOrder
{
    PopularityDesc,
    RatingDesc,
    ReleaseDateDesc,
    TitleAsc
}
