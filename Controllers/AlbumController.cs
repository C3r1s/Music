using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Extensions;
using Music.Models;
using Music.Models.Viewmodels;

namespace Music.Controllers;

[Authorize]
public class AlbumController(IAlbumRepository _albumRepository, IFavouriteRepository _favouriteRepository) : Controller
{
    private const int PageSize = 4;


    public async Task<IActionResult> Index(int page = 1)
    {
        var totalItems = await _albumRepository.GetCountAsync(); // Добавьте GetCountAsync в IAlbumRepository
        var albumsOnPage = await _albumRepository.GetAllPagedAsync((page - 1) * PageSize, PageSize);

        var userId = User.GetUserId();

        var favouriteAlbumIds = new HashSet<int>(await _favouriteRepository.GetFavouriteAlbumsIds(userId));


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
        var album = await _albumRepository.GetDetailsByIdAsync(id);
        var userId = User.GetUserId();

        var isInFavourites = await _favouriteRepository.IsAlbumInFavourites(userId, id);

        ViewBag.IsInFavourites = isInFavourites;

        return View(album);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var album = await _albumRepository.GetDetailsByIdAsync(id);
        if (album == null)
            return NotFound();

        return View(album);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Album album)
    {
        if (!ModelState.IsValid)
        {
            await _albumRepository.UpdateAsync(album);
            return RedirectToAction(nameof(Index));
        }

        return View(album);
    }
}