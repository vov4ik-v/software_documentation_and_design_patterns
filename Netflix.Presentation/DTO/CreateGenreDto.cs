using System.ComponentModel.DataAnnotations;

namespace Netflix.Presentation.DTO;

public record CreateGenreDto
{
    [Required] [MaxLength(100)] public string Name { get; init; } = string.Empty;
}