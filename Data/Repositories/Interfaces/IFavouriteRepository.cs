﻿using Music.Models;

namespace Music.Data.Repositories.Interfaces;

public interface IFavouriteRepository
{
    Task AddArtistToFavourites(int userId, int artistId);
    Task RemoveArtistFromFavourites(int userId, int artistId);

    Task AddAlbumToFavourites(int userId, int albumId);
    Task RemoveAlbumFromFavourites(int userId, int albumId);
    Task<List<Artist>> GetFavouriteArtists(int userId);
    Task<HashSet<int>> GetFavouriteAlbumsIds(int userId);
    Task<List<Album>> GetFavouriteAlbums(int userId);
    Task<List<Song>> GetFavouriteSongs(int userId);

    Task<bool> IsAlbumInFavourites(int userId, int albumId);
}