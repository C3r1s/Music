using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories.Interfaces;
using Music.Extensions;
using Music.Models;

namespace Music.Data.Repositories;

public class SongRepository(MusicDbContext context) : ISongRepository
{
    public async Task<List<Song>> GetAllByQueryAsync(string query, int skip, int take)
    {
        return await context.Songs
            .Where(s => s.Name.Contains(query))
            .AsNoTracking()
            .Include(s => s.Albums) // при необходимости
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }


    public async Task<Album> GetSongsByAlbumIdAsync(int albumId, int skip, int take)
    {
        var album = await context.Albums
            .AsNoTracking()
            .Include(a => a.Songs)
            .FirstOrDefaultAsync(a => a.Id == albumId);

        if (album == null)
            throw new KeyNotFoundException("Альбом не найден");

        album.Songs = album.Songs.Paginate(skip, take).ToList();
        return album;
    }

    public async Task<int> GetSongCountByAlbumIdAsync(int albumId)
    {
        return await context.Albums
            .Where(a => a.Id == albumId)
            .Select(a => a.Songs.Count)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetCountByQueryAsync(string query)
    {
        return await context.Songs
            .Where(s => s.Name.Contains(query))
            .CountAsync();
    }
}