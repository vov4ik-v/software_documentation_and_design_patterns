using System.ComponentModel.DataAnnotations;

namespace Netflix.WebClient.Models.DTO;

public record ContentDto(int Id, string Title, string Description, int ReleaseYear, string AgeRating, string Language, string Country, int GenreId, string GenreName, string ContentType, int? DurationMin, int? SeasonsCount, double AverageRating);

public record ContentDetailDto(int Id, string Title, string Description, int ReleaseYear, string AgeRating, string Language, string Country, int GenreId, string GenreName, string ContentType, int? DurationMin, int? SeasonsCount, double AverageRating, List<ReviewDto> Reviews, List<RatingDto> Ratings, List<EpisodeDto> Episodes);

public record GenreDto(int Id, string Name);
public record ReviewDto(int Id, string Comment, int UserId);
public record RatingDto(int Id, int Score, int UserId);
public record EpisodeDto(int Id, string Title, int SeasonNumber, int EpisodeNumber, int DurationMin);

public record CreateMovieDto
{
    [Required] [MaxLength(200)] public string Title { get; init; } = string.Empty;
    [Required] [MaxLength(2000)] public string Description { get; init; } = string.Empty;
    [Required] [Range(1900, 2100)] public int ReleaseYear { get; init; }
    [Required] [MaxLength(10)] public string AgeRating { get; init; } = string.Empty;
    [Required] [MaxLength(50)] public string Language { get; init; } = string.Empty;
    [Required] [MaxLength(100)] public string Country { get; init; } = string.Empty;
    [Required] public int GenreId { get; init; }
    [Required] [Range(1, 600)] public int DurationMin { get; init; }
}

public record UpdateMovieDto : CreateMovieDto
{
    [Required] public int Id { get; init; }
}

public record CreateSeriesDto
{
    [Required] [MaxLength(200)] public string Title { get; init; } = string.Empty;
    [Required] [MaxLength(2000)] public string Description { get; init; } = string.Empty;
    [Required] [Range(1900, 2100)] public int ReleaseYear { get; init; }
    [Required] [MaxLength(10)] public string AgeRating { get; init; } = string.Empty;
    [Required] [MaxLength(50)] public string Language { get; init; } = string.Empty;
    [Required] [MaxLength(100)] public string Country { get; init; } = string.Empty;
    [Required] public int GenreId { get; init; }
    [Required] [Range(1, 50)] public int SeasonsCount { get; init; }
}

public record UpdateSeriesDto : CreateSeriesDto
{
    [Required] public int Id { get; init; }
}

public record CreateGenreDto
{
    [Required] [MaxLength(100)] public string Name { get; init; } = string.Empty;
}

public record UpdateGenreDto : CreateGenreDto
{
    [Required] public int Id { get; init; }
}
