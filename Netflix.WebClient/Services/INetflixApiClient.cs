using Netflix.WebClient.Models.DTO;

namespace Netflix.WebClient.Services;

public interface INetflixApiClient
{
    Task<IEnumerable<ContentDto>> GetMoviesAsync();
    Task<ContentDetailDto?> GetMovieAsync(int id);
    Task<ContentDto?> CreateMovieAsync(CreateMovieDto dto);
    Task UpdateMovieAsync(UpdateMovieDto dto);
    Task DeleteMovieAsync(int id);

    Task<IEnumerable<ContentDto>> GetSeriesAsync();
    Task<ContentDetailDto?> GetSeriesAsync(int id);
    Task<ContentDto?> CreateSeriesAsync(CreateSeriesDto dto);
    Task UpdateSeriesAsync(UpdateSeriesDto dto);
    Task DeleteSeriesAsync(int id);

    Task<IEnumerable<GenreDto>> GetGenresAsync();
    Task<GenreDto?> GetGenreAsync(int id);
    Task<GenreDto?> CreateGenreAsync(CreateGenreDto dto);
    Task UpdateGenreAsync(UpdateGenreDto dto);
    Task DeleteGenreAsync(int id);
}
