using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories.Interfaces;
using Music.Models;

namespace Music.Data.Repositories;

public class SongRepository (MusicDbContext context) : ISongRepository
{
    public async Task<Album> GetAlbumWithSongsByIdAsync(int albumId) => (await context.Albums
        .AsNoTracking()
        .Include(a => a.Songs)
        .FirstOrDefaultAsync(a => a.Id == albumId))!;
}