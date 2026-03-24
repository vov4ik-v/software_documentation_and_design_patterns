using Microsoft.EntityFrameworkCore;
using Netflix.BusinessLogic.Interfaces;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.BusinessLogic.Services;

public class CatalogService(
    IContentRepository contentRepository,
    IGenreRepository genreRepository,
    IReviewRepository reviewRepository,
    IRatingRepository ratingRepository,
    NetflixDbContext context)
    : ICatalogService
{
    public async Task<IEnumerable<Content>> GetAllContentAsync()
    {
        return await contentRepository.GetAllAsync();
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        return await contentRepository.GetAllMoviesAsync();
    }

    public async Task<List<Series>> GetAllSeriesAsync()
    {
        return await contentRepository.GetAllSeriesAsync();
    }

    public async Task<IEnumerable<Content>> SearchContentByTitleAsync(string title)
    {
        var all = await contentRepository.GetAllAsync();
        return all.Where(c => c.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Content>> GetContentByGenreAsync(string genreName)
    {
        var all = await contentRepository.GetAllAsync();
        return all.Where(c => c.Genre != null && c.Genre.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Content?> GetContentDetailsAsync(int id)
    {
        return await contentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        return await genreRepository.GetAllAsync();
    }

    public async Task<Genre?> GetGenreByIdAsync(int id)
    {
        var genres = await genreRepository.GetAllAsync();
        return genres.FirstOrDefault(g => g.Id == id);
    }

    public async Task<IEnumerable<Review>> GetReviewsForContentAsync(int contentId)
    {
        return await reviewRepository.GetByContentIdAsync(contentId);
    }

    public async Task<IEnumerable<Rating>> GetRatingsForContentAsync(int contentId)
    {
        return await ratingRepository.GetByContentIdAsync(contentId);
    }

    public async Task<Movie> CreateMovieAsync(Movie movie)
    {
        await contentRepository.AddAsync(movie);
        await contentRepository.SaveChangesAsync();
        return movie;
    }

    public async Task<Series> CreateSeriesAsync(Series series)
    {
        await contentRepository.AddAsync(series);
        await contentRepository.SaveChangesAsync();
        return series;
    }

    public async Task UpdateMovieAsync(Movie movie)
    {
        var existing = await context.Movies.FindAsync(movie.Id)
            ?? throw new KeyNotFoundException($"Movie {movie.Id} not found.");
            
        context.Entry(existing).CurrentValues.SetValues(new
        {
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            AgeRating = movie.AgeRating,
            Language = movie.Language,
            Country = movie.Country,
            GenreId = movie.GenreId,
            DurationMin = movie.DurationMin
        });
        await context.SaveChangesAsync();
    }

    public async Task UpdateSeriesAsync(Series series)
    {
        var existing = await context.Series.FindAsync(series.Id)
            ?? throw new KeyNotFoundException($"Series {series.Id} not found.");
            
        context.Entry(existing).CurrentValues.SetValues(new
        {
            Title = series.Title,
            Description = series.Description,
            ReleaseYear = series.ReleaseYear,
            AgeRating = series.AgeRating,
            Language = series.Language,
            Country = series.Country,
            GenreId = series.GenreId,
            SeasonsCount = series.SeasonsCount
        });
        await context.SaveChangesAsync();
    }

    public async Task DeleteContentAsync(int id)
    {
        await contentRepository.DeleteAsync(id);
        await contentRepository.SaveChangesAsync();
    }

    public async Task<Genre> CreateGenreAsync(Genre genre)
    {
        await context.Genres.AddAsync(genre);
        await context.SaveChangesAsync();
        return genre;
    }

    public async Task UpdateGenreAsync(Genre genre)
    {
        var existing = await context.Genres.FindAsync(genre.Id)
            ?? throw new KeyNotFoundException($"Genre {genre.Id} not found.");
            
        context.Entry(existing).CurrentValues.SetValues(new { Name = genre.Name });
        await context.SaveChangesAsync();
    }

    public async Task DeleteGenreAsync(int id)
    {
        var genre = await context.Genres.FindAsync(id);
        if (genre != null)
        {
            context.Genres.Remove(genre);
            await context.SaveChangesAsync();
        }
    }
}
