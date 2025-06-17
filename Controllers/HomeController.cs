using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    private const int PageSize = 3;

    public async Task<IActionResult> Index()
    {
        var artists = await artistRepository.GetAllPagedAsync((1 - 1) * PageSize, PageSize);
        var userId = User.GetUserId();

        HashSet<int> favouriteArtistsIds = [];

        if (userId != 0)
            favouriteArtistsIds = (await favouriteRepository.GetFavouriteArtists(userId))
                .Select(a => a.Id).ToHashSet();

        var model = new HomeIndexViewModel
        {
            Artists = artists,
            FavouriteArtistIds = favouriteArtistsIds
        };

        return View(model);
    }


    public async Task<IActionResult> Search(string query, int page = 1)
    {
        query = (query ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(query))
            return RedirectToAction(nameof(Index));

        var artists = await artistRepository.GetAllByQueryAsync(query, (page - 1) * PageSize, PageSize);
        var albums = await albumRepository.GetAllByQueryAsync(query, (page - 1) * PageSize, PageSize);
        var songs = await songRepository.GetAllByQueryAsync(query, (page - 1) * PageSize, PageSize);


        var totalArtists = await artistRepository.GetCountByQueryAsync(query);
        var totalAlbums = await albumRepository.GetCountByQueryAsync(query);
        var totalSongs = await songRepository.GetCountByQueryAsync(query);

        var model = new SearchViewModel
        {
            Query = query,
            Artists = artists,
            Albums = albums,
            Songs = songs,
            Pagination = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = PageSize,
                TotalItems = Math.Max(Math.Max(totalArtists, totalAlbums), totalSongs),
                Query = query
            }
        };
        return View(model);
    }
}