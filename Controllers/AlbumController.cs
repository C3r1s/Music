using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Extensions;
using Music.Models.Viewmodels;

namespace Music.Controllers;

[Authorize]
public class AlbumController(
    IAlbumRepository albumRepository,
    IFavouriteRepository favouriteRepository)
    : Controller
{
    private const int PageSize = 5;
    public async Task<IActionResult> Index(int page = 1)
    {
        var totalItems = await albumRepository.GetCountAsync(); // Добавьте GetCountAsync в IAlbumRepository
        var albumsOnPage = await albumRepository.GetAllPagedAsync((page - 1) * PageSize, PageSize);

        var userId = User.GetUserId();

        var favouriteAlbumIds = new HashSet<int>(await favouriteRepository.GetFavouriteAlbumsIds(userId));


        var model = new AlbumIndexViewModel
        {
            Albums = albumsOnPage,
            Pagination = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = PageSize,
                TotalItems = totalItems
            },
            FavouriteAlbumIds = favouriteAlbumIds
        };

        return View(model);
    }
    public async Task<IActionResult> Details(int id, string name)
    {
        var album = await albumRepository.GetDetailsByIdAsync(id);
        var userId = User.GetUserId();

        var isInFavourites = await favouriteRepository.IsAlbumInFavourites(userId, id);

        ViewBag.IsInFavourites = isInFavourites;

        return View(album);
    }
}