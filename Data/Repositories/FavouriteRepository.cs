using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories.Interfaces;
using Music.Models;
using Music.Models.RelationModels;

namespace Music.Data.Repositories;

public class FavouriteRepository(MusicDbContext context) : IFavouriteRepository
{
    public async Task AddArtistToFavourites(int userId, int artistId)
    {
        await context.UserArtists.AddAsync(new UserArtist { UserId = userId, ArtistId = artistId });
        await context.SaveChangesAsync();
    }

    public async Task RemoveArtistFromFavourites(int userId, int artistId)
    {
        var entity =
            await context.UserArtists.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ArtistId == artistId);
        if (entity != null)
        {
            context.UserArtists.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task AddAlbumToFavourites(int userId, int albumId)
    {
        await context.UserAlbums.AddAsync(new UserAlbum { UserId = userId, AlbumId = albumId });
        await context.SaveChangesAsync();
    }

    public async Task RemoveAlbumFromFavourites(int userId, int albumId)
    {
        var entity = await context.UserAlbums.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AlbumId == albumId);
        if (entity != null)
        {
            context.UserAlbums.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task AddSongToFavourites(int userId, int songId)
    {
        await context.UserSongs.AddAsync(new UserSong { UserId = userId, SongId = songId });
        await context.SaveChangesAsync();
    }

    public async Task RemoveSongFromFavourites(int userId, int songId)
    {
        var entity = await context.UserSongs.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.SongId == songId);
        if (entity != null)
        {
            context.UserSongs.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<Artist>> GetFavouriteArtists(int userId)
    {
        return await context.UserArtists
            .Where(ua => ua.UserId == userId)
            .Select(ua => ua.Artist)
            .ToListAsync();
    }

    public async Task<List<Album>> GetFavouriteAlbums(int userId)
    {
        return await context.UserAlbums
            .Where(ua => ua.UserId == userId)
            .Select(ua => ua.Album)
            .ToListAsync();
    }

    public async Task<List<Song>> GetFavouriteSongs(int userId)
    {
        return await context.UserSongs
            .Where(ua => ua.UserId == userId)
            .Select(ua => ua.Song)
            .ToListAsync();
    }

    public async Task<bool> IsAlbumInFavourites(int userId, int albumId)
    {
        return await context.UserAlbums
            .AnyAsync(ua => ua.UserId == userId && ua.AlbumId == albumId);
    }
}