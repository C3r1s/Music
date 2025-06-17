using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Extensions;
using Music.Helper;
using Music.Models.Viewmodels;

namespace Music.Controllers;

[Authorize]
public class FavouritesController(IFavouriteRepository favouriteRepository)
    : Controller
{
    private const int PageSize = 5;

    public async Task<IActionResult> Index(int page = 1)
    {
        var userId = User.GetUserId();
        var artists = await favouriteRepository.GetFavouriteArtists(userId);
        var albums = await favouriteRepository.GetFavouriteAlbums(userId);
        var songs = await favouriteRepository.GetFavouriteSongs(userId);

        var model = new FavouritesViewModel
        {
            Artists = artists,
            Albums = albums,
            Songs = songs,
            Pagination = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = PageSize,
                TotalItems = Math.Max(Math.Max(artists.Count, albums.Count), songs.Count)
            }
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddArtist(int id)
    {
        var userId = User.GetUserId();
        await favouriteRepository.AddArtistToFavourites(userId, id);
        return RedirectToAction(nameof(HomeController.Index), ControllerHelper.GetName<HomeController>());
    }

    [HttpPost]
    public async Task<IActionResult> AddAlbum(int id, string returnUrl)
    {
        var userId = User.GetUserId();

        await favouriteRepository.AddAlbumToFavourites(userId, id);
        return RedirectToAction(nameof(AlbumController.Index), ControllerHelper.GetName<AlbumController>());
    }

    [HttpPost]
    public async Task<IActionResult> RemoveAlbum(int id)
    {
        var userId = User.GetUserId();

        await favouriteRepository.RemoveAlbumFromFavourites(userId, id);
        return RedirectToAction(nameof(AlbumController.Index), ControllerHelper.GetName<AlbumController>());
    }

    [HttpPost]
    public async Task<IActionResult> RemoveArtist(int id)
    {
        var userId = User.GetUserId();

        await favouriteRepository.RemoveArtistFromFavourites(userId, id);
        return RedirectToAction(nameof(HomeController.Index), ControllerHelper.GetName<HomeController>());
    }
}