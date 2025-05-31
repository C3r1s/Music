using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories;

namespace Music.Controllers;

public class HomeController(MusicDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var artists = await context.Artists.AsNoTracking().ToListAsync();

        return View(artists);
    }
}
