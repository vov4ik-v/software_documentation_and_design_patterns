using Microsoft.EntityFrameworkCore;
using Netflix.DataAccess.Data;
using Netflix.DataAccess.Interfaces;
using Netflix.Domain.Entities;

namespace Netflix.DataAccess.Repositories;

public class ContentRepository(NetflixDbContext context) : IContentRepository
{
    public async Task<Content?> GetByIdAsync(int id)
    {
        return await context.Contents
            .Include(c => c.Genre)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Content>> GetAllAsync()
    {
        return await context.Contents
            .Include(c => c.Genre)
            .ToListAsync();
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        return await context.Movies
            .Include(c => c.Genre)
            .ToListAsync();
    }

    public async Task<List<Series>> GetAllSeriesAsync()
    {
        return await context.Series
            .Include(c => c.Genre)
            .ToListAsync();
    }

    public async Task AddAsync(Content content)
    {
        await context.Contents.AddAsync(content);
    }

    public async Task AddRangeAsync(IEnumerable<Content> contents)
    {
        await context.Contents.AddRangeAsync(contents);
    }

    public Task UpdateAsync(Content content)
    {
        context.Contents.Update(content);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var content = await context.Contents.FindAsync(id);
        if (content != null)
            context.Contents.Remove(content);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}