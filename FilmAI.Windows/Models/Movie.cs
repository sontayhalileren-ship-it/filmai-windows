using System;
using System.Collections.Generic;

namespace FilmAI.Windows.Models;

/// <summary>
/// Bir filmin temel verisi. Not: Bu model kasıtlı olarak "AiMatchPercentage" gibi
/// AI tabanlı hiçbir alan içermez — eşleştirme/puanlama tamamen filtre ve
/// IMDb/TMDB verisine dayanır.
/// </summary>
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public string PosterPath { get; set; } = string.Empty;
    public string BackdropPath { get; set; } = string.Empty;
    public double Rating { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int RuntimeMinutes { get; set; }
    public string AgeRating { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();
    public List<CastMember> Cast { get; set; } = new();
    public List<CrewMember> Crew { get; set; } = new();
    public string Director { get; set; } = string.Empty;
    public bool IsHiddenGem { get; set; }
    public bool IsInWatchlist { get; set; }
    public bool IsWatched { get; set; }
    public int? UserRating { get; set; }
    public string? UserNote { get; set; }
}
