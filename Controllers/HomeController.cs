using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories;
using Music.Models.Viewmodels;
namespace Music.Controllers;

[Authorize]
public class HomeController(MusicDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var artists = await context.Artists.AsNoTracking().ToListAsync();
        return View(artists);
    }

    private const int PageSize = 5;

    public async Task<IActionResult> Search(string query, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(query))
            return RedirectToAction(nameof(Index));

        var artists = await context.Artists
            .Where(a => a.Name.Contains(query))
            .AsNoTracking()
            .ToListAsync();

        var albums = await context.Albums
            .Where(a => a.Name.Contains(query))
            .AsNoTracking()
            .Include(a => a.Songs)
            .ToListAsync();

        var songs = await context.Songs
            .Where(s => s.Name.Contains(query))
            .AsNoTracking()
            .ToListAsync();

        var totalArtists = artists.Count;
        var totalAlbums = albums.Count;
        var totalSongs = songs.Count;

        var model = new SearchViewModel
        {
            Query = query,
            Artists = artists.Paginate(page, PageSize).ToList(),
            Albums = albums.Paginate(page, PageSize).ToList(),
            Songs = songs.Paginate(page, PageSize).ToList()
        };

        ViewBag.Pagination = new PaginationViewModel
        {
            PageNumber = page,
            PageSize = PageSize,
            TotalItems = Math.Max(Math.Max(totalArtists, totalAlbums), totalSongs)
        };

        return View(model);
    }
}