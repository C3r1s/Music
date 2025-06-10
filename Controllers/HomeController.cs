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
    private const int PageSize = 5;

    public async Task<IActionResult> Index()
    {
        var artists = await artistRepository.GetAllAsync();
        var userId = User.GetUserId();

        HashSet<int> favouriteArtistsIds = [];

        if (userId != 0)
        {
            favouriteArtistsIds = (await favouriteRepository.GetFavouriteArtists(userId))
                .Select(a => a.Id).ToHashSet();
        }
        var model = new HomeIndexViewModel
        {
            Artists = artists,
            FavouriteArtistIds = favouriteArtistsIds
        };

        return View(model);
    }


    public async Task<IActionResult> Search(string query, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(query))
            return RedirectToAction(nameof(Index));

        var artists = await artistRepository.GetAllByQueryAsync(query, (page - 1) * PageSize, PageSize);
        var albums = await albumRepository.GetAllByQueryAsync(query, (page - 1) * PageSize, PageSize);
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
            Pagination = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = PageSize,
                TotalItems = Math.Max(Math.Max(totalArtists, totalAlbums), totalSongs)
            }
        };
        return View(model);
    }
}