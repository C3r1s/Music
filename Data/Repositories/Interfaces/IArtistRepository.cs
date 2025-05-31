using Music.Models;

namespace Music.Data.Repositories.Interfaces;

public interface IArtistRepository
{
    Task<List<Artist>> GetAllAsync();
}