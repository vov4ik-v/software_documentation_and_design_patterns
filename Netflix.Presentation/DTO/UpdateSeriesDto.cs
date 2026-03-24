using System.ComponentModel.DataAnnotations;

namespace Netflix.Presentation.DTO;

public record UpdateSeriesDto
{
    [Required] public int Id { get; init; }

    [Required] [MaxLength(200)] public string Title { get; init; } = string.Empty;

    [Required] [MaxLength(2000)] public string Description { get; init; } = string.Empty;

    [Required] [Range(1900, 2100)] public int ReleaseYear { get; init; }

    [Required] [MaxLength(10)] public string AgeRating { get; init; } = string.Empty;

    [Required] [MaxLength(50)] public string Language { get; init; } = string.Empty;

    [Required] [MaxLength(100)] public string Country { get; init; } = string.Empty;

    [Required] public int GenreId { get; init; }

    [Required] [Range(1, 50)] public int SeasonsCount { get; init; }
}