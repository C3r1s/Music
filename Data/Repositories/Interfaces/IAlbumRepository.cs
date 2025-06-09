using Music.Models;

namespace Music.Data.Repositories.Interfaces;

public interface IAlbumRepository
{
    Task<List<Album>> GetAllAsync();
    Task<Album> GetDetailsByIdAsync(int id);

    Task<List<Album>> GetAllByQueryAsync(string query);
}