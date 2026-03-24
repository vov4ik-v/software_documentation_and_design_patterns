using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Domain.Entities;
using Netflix.Presentation.DTO;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController(ICatalogService catalogService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
    {
        var genres = await catalogService.GetAllGenresAsync();
        return Ok(mapper.Map<IEnumerable<GenreDto>>(genres));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreDto>> GetGenre(int id)
    {
        var genre = await catalogService.GetGenreByIdAsync(id);
        if (genre == null) return NotFound($"Genre with ID {id} not found.");

        return Ok(mapper.Map<GenreDto>(genre));
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> CreateGenre(CreateGenreDto dto)
    {
        var genre = mapper.Map<Genre>(dto);
        await catalogService.CreateGenreAsync(genre);
        return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, mapper.Map<GenreDto>(genre));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGenre(int id, UpdateGenreDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        try
        {
            var genre = mapper.Map<Genre>(dto);
            await catalogService.UpdateGenreAsync(genre);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        await catalogService.DeleteGenreAsync(id);
        return NoContent();
    }
}