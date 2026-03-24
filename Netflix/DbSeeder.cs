using Microsoft.EntityFrameworkCore;
using Netflix.DataAccess.Data;
using Netflix.Domain.Entities;

namespace Netflix;

public static class DbSeeder
{
    public static async Task SeedAsync(NetflixDbContext context)
    {
        if (await context.Genres.AnyAsync())
            return;

        var genres = new List<Genre>
        {
            new() { Name = "Action" },
            new() { Name = "Drama" },
            new() { Name = "Comedy" },
            new() { Name = "Thriller" },
            new() { Name = "Sci-Fi" },
            new() { Name = "Documentary" }
        };
        await context.Genres.AddRangeAsync(genres);
        await context.SaveChangesAsync();

        var movies = new List<Movie>
        {
            new()
            {
                Title = "Inception",
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                ReleaseYear = 2010, AgeRating = "PG-13", Language = "English", Country = "USA", GenreId = genres[4].Id,
                DurationMin = 148
            },
            new()
            {
                Title = "The Dark Knight", Description = "When the Joker emerges, Batman must confront chaos.",
                ReleaseYear = 2008, AgeRating = "PG-13", Language = "English", Country = "USA", GenreId = genres[0].Id,
                DurationMin = 152
            },
            new()
            {
                Title = "Interstellar", Description = "A team of explorers travel through a wormhole in space.",
                ReleaseYear = 2014, AgeRating = "PG-13", Language = "English", Country = "USA", GenreId = genres[4].Id,
                DurationMin = 169
            },
            new()
            {
                Title = "The Shawshank Redemption", Description = "Two imprisoned men bond over years in prison.",
                ReleaseYear = 1994, AgeRating = "R", Language = "English", Country = "USA", GenreId = genres[1].Id,
                DurationMin = 142
            },
            new()
            {
                Title = "Parasite", Description = "A poor family schemes to become employed by a wealthy family.",
                ReleaseYear = 2019, AgeRating = "R", Language = "Korean", Country = "South Korea",
                GenreId = genres[3].Id, DurationMin = 132
            },
            new()
            {
                Title = "The Grand Budapest Hotel",
                Description = "Adventures of a legendary concierge and his lobby boy.", ReleaseYear = 2014,
                AgeRating = "R", Language = "English", Country = "Germany", GenreId = genres[2].Id, DurationMin = 100
            }
        };
        await context.Movies.AddRangeAsync(movies);

        var series = new List<Series>
        {
            new()
            {
                Title = "Breaking Bad", Description = "A chemistry teacher turns to producing methamphetamine.",
                ReleaseYear = 2008, AgeRating = "TV-MA", Language = "English", Country = "USA", GenreId = genres[3].Id,
                SeasonsCount = 5
            },
            new()
            {
                Title = "Stranger Things", Description = "Supernatural events unfold in a small Indiana town.",
                ReleaseYear = 2016, AgeRating = "TV-14", Language = "English", Country = "USA", GenreId = genres[4].Id,
                SeasonsCount = 4
            },
            new()
            {
                Title = "The Crown", Description = "Chronicles the reign of Queen Elizabeth II.", ReleaseYear = 2016,
                AgeRating = "TV-MA", Language = "English", Country = "UK", GenreId = genres[1].Id, SeasonsCount = 6
            }
        };
        await context.Series.AddRangeAsync(series);

        await context.SaveChangesAsync();
    }
}