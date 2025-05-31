using Music.Models;

namespace Music.Data.Repositories.Interfaces;

public interface ISongRepository
{
    Task<Album> GetAlbumWithSongsByIdAsync(int albumId);
}