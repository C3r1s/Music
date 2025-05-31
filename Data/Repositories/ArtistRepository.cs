using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories.Interfaces;
using Music.Models;

namespace Music.Data.Repositories;

public class ArtistRepository(MusicDbContext context) : IArtistRepository
{
    public async Task<List<Artist>> GetAllAsync() => await context.Artists.AsNoTracking().ToListAsync();
    
}