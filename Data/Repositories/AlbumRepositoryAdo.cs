using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories.Interfaces;
using Music.Models;

namespace Music.Data.Repositories;

public class AlbumRepositoryAdo(MusicDbContext context) : IAlbumRepository
{
    public async Task<List<Album>> GetAllAsync()
    {
        var albums = await context.Albums.AsNoTracking().ToListAsync();

        return albums;
    }

    public async Task<Album> GetDetailsByIdAsync(int id)
    {
        var album = await context.Albums
                            .AsNoTracking()
                            .Include(album => album.Songs)
                            .FirstAsync(x => x.Id == id);

        return album;
    }
}