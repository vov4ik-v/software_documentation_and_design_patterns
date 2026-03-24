using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Domain.Entities;
using Netflix.Presentation.DTO;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(ICatalogService catalogService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContentDto>>> GetMovies()
    {
        var movies = await catalogService.GetAllMoviesAsync();
        return Ok(mapper.Map<IEnumerable<ContentDto>>(movies));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContentDetailDto>> GetMovie(int id)
    {
        var content = await catalogService.GetContentDetailsAsync(id);
        if (content is not Movie movie)
            return NotFound($"Movie with ID {id} not found.");

        var reviews = await catalogService.GetReviewsForContentAsync(id);
        var ratings = await catalogService.GetRatingsForContentAsync(id);

        movie.Reviews = reviews.ToList();
        movie.Ratings = ratings.ToList();

        return Ok(mapper.Map<ContentDetailDto>(movie));
    }

    [HttpPost]
    public async Task<ActionResult<ContentDto>> CreateMovie(CreateMovieDto dto)
    {
        var movie = mapper.Map<Movie>(dto);
        await catalogService.CreateMovieAsync(movie);

        var created = await catalogService.GetContentDetailsAsync(movie.Id);
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, mapper.Map<ContentDto>(created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMovie(int id, UpdateMovieDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        try
        {
            var movie = mapper.Map<Movie>(dto);
            await catalogService.UpdateMovieAsync(movie);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        await catalogService.DeleteContentAsync(id);
        return NoContent();
    }
}