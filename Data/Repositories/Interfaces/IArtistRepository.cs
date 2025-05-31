using Music.Models;

namespace Music.Data.Repositories.Interfaces;

public interface IArtistRepository
{
    Task<List<Artist>> GetAllAsync();
    Task<Artist> GetByIdAsync(int id);
    Task AddAsync(Artist artist);
    Task UpdateAsync(Artist artist);
    Task DeleteAsync(int id);
    Task<Artist> GetByIdWithAlbumsAndSongsAsync(int id);
}