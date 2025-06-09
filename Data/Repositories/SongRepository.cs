using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories.Interfaces;
using Music.Models;

namespace Music.Data.Repositories;

public class SongRepository(MusicDbContext context) : ISongRepository
{
    public async Task<Album> GetAlbumWithSongsByIdAsync(int albumId)
    {
        return (await context.Albums
            .AsNoTracking()
            .Include(a => a.Songs)
            .FirstOrDefaultAsync(a => a.Id == albumId))!;
    }
    
    public async Task<List<Song>> GetAllByQueryAsync(string query)
    {
        return await context.Songs
            .Where(s => s.Name.Contains(query))
            .AsNoTracking()
            .ToListAsync();
    }
}