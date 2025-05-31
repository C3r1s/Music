using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories;
using Music.Models;

namespace Music.Controllers;

public class HomeController(MusicDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var artists = await context.Artists.AsNoTracking().ToListAsync();
        return View(artists);
    }

    public async Task<IActionResult> Search(string query)
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

        var model = new SearchViewModel
        {
            Query = query,
            Artists = artists,
            Albums = albums,
            Songs = songs
        };

        return View(model);
    }
}

