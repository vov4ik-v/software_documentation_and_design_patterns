using Netflix.Domain.Entities;

namespace Netflix.BusinessLogic.Interfaces;

public interface ICatalogService
{
    Task<IEnumerable<Content>> GetAllContentAsync();
    Task<List<Movie>> GetAllMoviesAsync();
    Task<List<Series>> GetAllSeriesAsync();
    Task<IEnumerable<Content>> SearchContentByTitleAsync(string title);
    Task<IEnumerable<Content>> GetContentByGenreAsync(string genreName);
    Task<Content?> GetContentDetailsAsync(int id);
    Task<IEnumerable<Genre>> GetAllGenresAsync();
    Task<Genre?> GetGenreByIdAsync(int id);
    Task<IEnumerable<Review>> GetReviewsForContentAsync(int contentId);
    Task<IEnumerable<Rating>> GetRatingsForContentAsync(int contentId);

    Task<Movie> CreateMovieAsync(Movie movie);
    Task<Series> CreateSeriesAsync(Series series);
    Task UpdateMovieAsync(Movie movie);
    Task UpdateSeriesAsync(Series series);
    Task DeleteContentAsync(int id);

    Task<Genre> CreateGenreAsync(Genre genre);
    Task UpdateGenreAsync(Genre genre);
    Task DeleteGenreAsync(int id);
}