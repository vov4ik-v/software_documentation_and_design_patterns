using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Netflix.BusinessLogic.Interfaces;
using Netflix.Domain.Entities;
using Netflix.Presentation.DTO;

namespace Netflix.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeriesController(ICatalogService catalogService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContentDto>>> GetSeries()
    {
        var series = await catalogService.GetAllSeriesAsync();
        return Ok(mapper.Map<IEnumerable<ContentDto>>(series));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContentDetailDto>> GetSeries(int id)
    {
        var content = await catalogService.GetContentDetailsAsync(id);
        if (content is not Series s)
            return NotFound($"Series with ID {id} not found.");

        var reviews = await catalogService.GetReviewsForContentAsync(id);
        var ratings = await catalogService.GetRatingsForContentAsync(id);

        s.Reviews = reviews.ToList();
        s.Ratings = ratings.ToList();

        return Ok(mapper.Map<ContentDetailDto>(s));
    }

    [HttpPost]
    public async Task<ActionResult<ContentDto>> CreateSeries(CreateSeriesDto dto)
    {
        var series = mapper.Map<Series>(dto);
        await catalogService.CreateSeriesAsync(series);

        var created = await catalogService.GetContentDetailsAsync(series.Id);
        return CreatedAtAction(nameof(GetSeries), new { id = series.Id }, mapper.Map<ContentDto>(created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSeries(int id, UpdateSeriesDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        try
        {
            var series = mapper.Map<Series>(dto);
            await catalogService.UpdateSeriesAsync(series);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSeries(int id)
    {
        await catalogService.DeleteContentAsync(id);
        return NoContent();
    }
}