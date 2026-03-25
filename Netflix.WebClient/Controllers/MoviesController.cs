using Microsoft.AspNetCore.Mvc;
using Netflix.WebClient.Models.DTO;
using Netflix.WebClient.Services;

namespace Netflix.WebClient.Controllers;

public class MoviesController(INetflixApiClient apiClient) : Controller
{
    public async Task<IActionResult> Index()
    {
        var movies = await apiClient.GetMoviesAsync();
        return View(movies);
    }

    public async Task<IActionResult> Details(int id)
    {
        var movie = await apiClient.GetMovieAsync(id);
        if (movie == null) return NotFound();
        return View(movie);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Genres = await apiClient.GetGenresAsync();
        return View(new CreateMovieDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMovieDto dto)
    {
        if (ModelState.IsValid)
        {
            await apiClient.CreateMovieAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Genres = await apiClient.GetGenresAsync();
        return View(dto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var movie = await apiClient.GetMovieAsync(id);
        if (movie == null) return NotFound();

        var dto = new UpdateMovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            ReleaseYear = movie.ReleaseYear,
            AgeRating = movie.AgeRating,
            Language = movie.Language,
            Country = movie.Country,
            GenreId = movie.GenreId,
            DurationMin = movie.DurationMin ?? 0
        };

        ViewBag.Genres = await apiClient.GetGenresAsync();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateMovieDto dto)
    {
        if (id != dto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await apiClient.UpdateMovieAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Genres = await apiClient.GetGenresAsync();
        return View(dto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var movie = await apiClient.GetMovieAsync(id);
        if (movie == null) return NotFound();
        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await apiClient.DeleteMovieAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
