using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories;
using Music.Models;

namespace Music.Controllers;

[Authorize]
public class SongController(MusicDbContext context) : Controller
{
    private const int PageSize = 5;

    public async Task<IActionResult> Index(int albumId, int page = 1)
    {
        var album = await context.Albums
            .AsNoTracking()
            .Include(a => a.Songs)
            .FirstOrDefaultAsync(a => a.Id == albumId);

        if (album == null)
            return NotFound();

        var totalSongs = album.Songs.Count;
        var songsOnPage = album.Songs.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        var pagination = new PaginationViewModel
        {
            PageNumber = page,
            PageSize = PageSize,
            TotalItems = totalSongs
        };

        ViewBag.Pagination = pagination;
        ViewBag.AlbumId = albumId;

        album.Songs = songsOnPage;
        return View(album);
    }
}