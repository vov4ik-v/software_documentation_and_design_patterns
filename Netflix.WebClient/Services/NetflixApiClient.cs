using System.Text;
using System.Text.Json;
using Netflix.WebClient.Models.DTO;

namespace Netflix.WebClient.Services;

public class NetflixApiClient(HttpClient httpClient) : INetflixApiClient
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<IEnumerable<ContentDto>> GetMoviesAsync()
    {
        var response = await httpClient.GetAsync("/api/movies");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ContentDto>>(content, _jsonOptions) ?? Enumerable.Empty<ContentDto>();
    }

    public async Task<ContentDetailDto?> GetMovieAsync(int id)
    {
        var response = await httpClient.GetAsync($"/api/movies/{id}");
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContentDetailDto>(content, _jsonOptions);
    }

    public async Task<ContentDto?> CreateMovieAsync(CreateMovieDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/api/movies", httpContent);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContentDto>(content, _jsonOptions);
    }

    public async Task UpdateMovieAsync(UpdateMovieDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"/api/movies/{dto.Id}", httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteMovieAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/movies/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<ContentDto>> GetSeriesAsync()
    {
        var response = await httpClient.GetAsync("/api/series");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ContentDto>>(content, _jsonOptions) ?? Enumerable.Empty<ContentDto>();
    }

    public async Task<ContentDetailDto?> GetSeriesAsync(int id)
    {
        var response = await httpClient.GetAsync($"/api/series/{id}");
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContentDetailDto>(content, _jsonOptions);
    }

    public async Task<ContentDto?> CreateSeriesAsync(CreateSeriesDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/api/series", httpContent);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContentDto>(content, _jsonOptions);
    }

    public async Task UpdateSeriesAsync(UpdateSeriesDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"/api/series/{dto.Id}", httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteSeriesAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/series/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IEnumerable<GenreDto>> GetGenresAsync()
    {
        var response = await httpClient.GetAsync("/api/genres");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<GenreDto>>(content, _jsonOptions) ?? Enumerable.Empty<GenreDto>();
    }

    public async Task<GenreDto?> GetGenreAsync(int id)
    {
        var response = await httpClient.GetAsync($"/api/genres/{id}");
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GenreDto>(content, _jsonOptions);
    }

    public async Task<GenreDto?> CreateGenreAsync(CreateGenreDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/api/genres", httpContent);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GenreDto>(content, _jsonOptions);
    }

    public async Task UpdateGenreAsync(UpdateGenreDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"/api/genres/{dto.Id}", httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteGenreAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"/api/genres/{id}");
        response.EnsureSuccessStatusCode();
    }
}
