using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories;

namespace Music.Controllers;

public class SongController(MusicDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var album = await context.Albums
                            .AsNoTracking()
                            .Include(album => album.Songs)
                            .FirstAsync(x => x.Id == 1);

        return View(album);
    }
}