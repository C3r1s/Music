using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Extensions;
using Music.Models.Viewmodels;

namespace Music.Controllers;

[Authorize]
public class FavouritesController(IFavouriteRepository favouriteRepository)
    : Controller
{
    private const int PageSize = 5;

    public async Task<IActionResult> Index(int page = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


        var artists = await favouriteRepository.GetFavouriteArtists(int.Parse(userId));
        var albums = await favouriteRepository.GetFavouriteAlbums(int.Parse(userId));
        var songs = await favouriteRepository.GetFavouriteSongs(int.Parse(userId));

        var model = new FavouritesViewModel
        {
            Artists = artists.Paginate(page, PageSize).ToList(),
            Albums = albums.Paginate(page, PageSize).ToList(),
            Songs = songs.Paginate(page, PageSize).ToList()
        };

        ViewBag.Pagination = new PaginationViewModel
        {
            PageNumber = page,
            PageSize = PageSize,
            TotalItems = Math.Max(Math.Max(artists.Count, albums.Count), songs.Count)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddArtist(int id, string returnUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await favouriteRepository.AddArtistToFavourites(int.Parse(userId), id);
        return Redirect(returnUrl ?? "/");
    }

    [HttpPost]
    public async Task<IActionResult> AddAlbum(int id, string returnUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await favouriteRepository.AddAlbumToFavourites(int.Parse(userId), id);
        return Redirect(returnUrl ?? "/");
    }

    [HttpPost]
    public async Task<IActionResult> AddSong(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await favouriteRepository.AddSongToFavourites(int.Parse(userId), id);
        return RedirectToAction("Index");
    }

    public async Task<bool> IsAlbumInFavourites(int userId, int albumId)
    {
        return await favouriteRepository.IsAlbumInFavourites(userId, albumId);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveAlbum(int id, string returnUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await favouriteRepository.RemoveAlbumFromFavourites(int.Parse(userId), id);
        return Redirect(returnUrl ?? "/");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveArtist(int id, string returnUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await favouriteRepository.RemoveArtistFromFavourites(int.Parse(userId), id);
        return Redirect(returnUrl ?? "/");
    }
}