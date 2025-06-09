using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories.Interfaces;
using Music.Models;

namespace Music.Data.Repositories;

public class ArtistRepository(MusicDbContext context) : IArtistRepository
{
    public async Task<List<Artist>> GetAllAsync()
    {
        return await context.Artists.AsNoTracking().ToListAsync();
    }

    public async Task<Artist> GetByIdAsync(int id)
    {
        return await context.Artists.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Artist artist)
    {
        context.Artists.Add(artist);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Artist artist)
    {
        context.Artists.Update(artist);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var artist = await context.Artists.FindAsync(id);
        if (artist != null)
        {
            context.Artists.Remove(artist);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Artist> GetByIdWithAlbumsAndSongsAsync(int id)
    {
        return await context.Artists
            .Include(a => a.Albums)
            .ThenInclude(album => album.Songs)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Artist>> GetAllByQueryAsync(string query)
    {
        return await context.Artists
            .Where(a => a.Name.Contains(query))
            .AsNoTracking()
            .ToListAsync();
    }
}