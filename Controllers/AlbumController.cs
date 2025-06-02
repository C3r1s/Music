using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Models;

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
        var albums = await albumRepository.GetAllAsync();
        var totalItems = albums.Count;

        var pagination = new PaginationViewModel
        {
            PageNumber = page,
            PageSize = PageSize,
            TotalItems = totalItems
        };

        var albumsOnPage = albums.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        ViewBag.Pagination = pagination;
        return View(albumsOnPage);
    }

    public async Task<IActionResult> Details(int id, string name)
    {
        var album = await albumRepository.GetDetailsByIdAsync(id);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var isInFavourites = await favouriteRepository.IsAlbumInFavourites(int.Parse(userId), id);

        ViewBag.IsInFavourites = isInFavourites;

        return View(album);
    }
}