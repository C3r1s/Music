using Music.Models;

namespace Music.Data.Repositories.Interfaces;

public interface ISongRepository
{
    Task<List<Song>> GetAllByQueryAsync(string query, int skip, int take);
    Task<Album> GetSongsByAlbumIdAsync(int albumId, int pageSize, int i);
    Task<int> GetSongCountByAlbumIdAsync(int albumId);
    Task<int> GetCountByQueryAsync(string query);
}