using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories;
using Music.Data.Repositories.Interfaces;
using Music.Extensions;
using Music.Models.Viewmodels;

namespace Music.Controllers;

[Authorize]
public class HomeController(
    IFavouriteRepository favouriteRepository,
    ISongRepository songRepository,
    IAlbumRepository albumRepository,
    IArtistRepository artistRepository) : Controller
{
    private const int PageSize = 5;

    public async Task<IActionResult> Index()
    {
        var artists = await artistRepository.GetAllAsync();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        List<int> favouriteArtistsIds = [];

        if (!string.IsNullOrEmpty(userId))
            favouriteArtistsIds = (await favouriteRepository.GetFavouriteArtists(int.Parse(userId)))
                .Select(a => a.Id)
                .ToList();

        ViewBag.FavouriteArtistIds = favouriteArtistsIds;
        return View(artists);
    }

    public async Task<IActionResult> Search(string query, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(query))
            return RedirectToAction(nameof(Index));

        var artists = await artistRepository.GetAllByQueryAsync(query);

        var albums = await albumRepository.GetAllByQueryAsync(query);

        var songs = await songRepository.GetAllByQueryAsync(query);


        var totalArtists = artists.Count;
        var totalAlbums = albums.Count;
        var totalSongs = songs.Count;

        var model = new SearchViewModel
        {
            Query = query,
            Artists = artists.Paginate(page, PageSize).ToList(),
            Albums = albums.Paginate(page, PageSize).ToList(),
            Songs = songs.Paginate(page, PageSize).ToList(),
            PaginationViewModel = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = PageSize,
                TotalItems = Math.Max(Math.Max(totalArtists, totalAlbums), totalSongs)
            }
        };

        ViewBag.Pagination = model.PaginationViewModel;

        return View(model);
    }
}