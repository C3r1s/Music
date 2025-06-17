using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Models.Viewmodels;

namespace Music.Controllers;

[Authorize]
public class SongController(ISongRepository songRepository) : Controller
{
    private const int PageSize = 5;

    public async Task<IActionResult> Index(int albumId, int page = 1)
    {
        var album = await songRepository.GetSongsByAlbumIdAsync(albumId, (page - 1) * PageSize, PageSize);
        if (album == null)
            return NotFound();

        var totalSongs = await songRepository.GetSongCountByAlbumIdAsync(albumId);

        var model = new SongIndexViewModel
        {
            Album = album,
            Pagination = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = PageSize,
                TotalItems = totalSongs
            }
        };

        return View(model);
    }
}