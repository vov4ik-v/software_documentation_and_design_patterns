using System.ComponentModel.DataAnnotations;

namespace Netflix.Presentation.DTO;

public record UpdateGenreDto
{
    [Required] public int Id { get; init; }

    [Required] [MaxLength(100)] public string Name { get; init; } = string.Empty;
}