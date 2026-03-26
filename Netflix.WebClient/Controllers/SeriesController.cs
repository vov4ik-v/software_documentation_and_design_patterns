using Microsoft.AspNetCore.Mvc;
using Netflix.WebClient.Models.DTO;
using Netflix.WebClient.Services;

namespace Netflix.WebClient.Controllers;

public class SeriesController(INetflixApiClient apiClient) : Controller
{
    public async Task<IActionResult> Index()
    {
        var series = await apiClient.GetSeriesAsync();
        return View(series);
    }

    public async Task<IActionResult> Details(int id)
    {
        var series = await apiClient.GetSeriesAsync(id);
        if (series == null) return NotFound();
        return View(series);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Genres = await apiClient.GetGenresAsync();
        return View(new CreateSeriesDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSeriesDto dto)
    {
        if (ModelState.IsValid)
        {
            await apiClient.CreateSeriesAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Genres = await apiClient.GetGenresAsync();
        return View(dto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var series = await apiClient.GetSeriesAsync(id);
        if (series == null) return NotFound();

        var dto = new UpdateSeriesDto
        {
            Id = series.Id,
            Title = series.Title,
            Description = series.Description,
            ReleaseYear = series.ReleaseYear,
            AgeRating = series.AgeRating,
            Language = series.Language,
            Country = series.Country,
            GenreId = series.GenreId,
            SeasonsCount = series.SeasonsCount ?? 0
        };

        ViewBag.Genres = await apiClient.GetGenresAsync();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateSeriesDto dto)
    {
        if (id != dto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await apiClient.UpdateSeriesAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Genres = await apiClient.GetGenresAsync();
        return View(dto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var series = await apiClient.GetSeriesAsync(id);
        if (series == null) return NotFound();
        return View(series);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await apiClient.DeleteSeriesAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
