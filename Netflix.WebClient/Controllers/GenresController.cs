using Microsoft.AspNetCore.Mvc;
using Netflix.WebClient.Models.DTO;
using Netflix.WebClient.Services;

namespace Netflix.WebClient.Controllers;

public class GenresController(INetflixApiClient apiClient) : Controller
{
    public async Task<IActionResult> Index()
    {
        var genres = await apiClient.GetGenresAsync();
        return View(genres);
    }

    public IActionResult Create()
    {
        return View(new CreateGenreDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGenreDto dto)
    {
        if (ModelState.IsValid)
        {
            await apiClient.CreateGenreAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var genre = await apiClient.GetGenreAsync(id);
        if (genre == null) return NotFound();

        var dto = new UpdateGenreDto
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateGenreDto dto)
    {
        if (id != dto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await apiClient.UpdateGenreAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var genre = await apiClient.GetGenreAsync(id);
        if (genre == null) return NotFound();
        return View(genre);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await apiClient.DeleteGenreAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
