using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories.Interfaces;
using Music.Models;

namespace Music.Data.Repositories;

public class AlbumRepository(MusicDbContext context) : IAlbumRepository
{
    public async Task<List<Album>> GetAllPagedAsync(int skip, int take)
    {
        return await context.Albums
            .AsNoTracking()
            .Include(a => a.Songs)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
        return await context.Albums.CountAsync();
    }

    public async Task<Album> GetDetailsByIdAsync(int id)
    {
        var album = await context.Albums
            .AsNoTracking()
            .Include(album => album.Songs)
            .FirstAsync(x => x.Id == id);

        return album;
    }

    public async Task<List<Album>> GetAllByQueryAsync(string query, int skip, int take)
    {
        return await context.Albums
            .Where(a => a.Name.Contains(query))
            .AsNoTracking()
            .Include(a => a.Songs)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task UpdateAsync(Album album)
    {
        context.Albums.Update(album);
        await context.SaveChangesAsync();
    }

    public Task<int> GetCountByQueryAsync(string query)
    {
        return context.Albums.CountAsync(a => a.Name.Contains(query));
    }
}