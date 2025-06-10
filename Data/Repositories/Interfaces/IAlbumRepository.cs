using Music.Models;

namespace Music.Data.Repositories.Interfaces;

public interface IAlbumRepository
{
    Task<List<Album>> GetAllPagedAsync(int skip, int take);
    Task<Album> GetDetailsByIdAsync(int id);
    Task<int> GetCountAsync();
    Task<List<Album>> GetAllByQueryAsync(string query, int skip, int take);
}