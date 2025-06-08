using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Models;
using Music.Models.Viewmodels;

namespace Music.Controllers;

[Authorize]
public class ArtistController(IArtistRepository artistRepository) : Controller
{
    private const int PageSize = 5;

    public async Task<IActionResult> Index(int page = 1)
    {
        var artists = await artistRepository.GetAllAsync();
        var totalItems = artists.Count;

        var pagination = new PaginationViewModel
        {
            PageNumber = page,
            PageSize = PageSize,
            TotalItems = totalItems
        };

        var artistsOnPage = artists.Paginate(page, PageSize).ToList();

        ViewBag.Pagination = pagination;
        return View(artistsOnPage);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Artist artist)
    {
        if (!ModelState.IsValid)
        {
            await artistRepository.AddAsync(artist);
            return RedirectToAction(nameof(Index));
        }

        return View(artist);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var artist = await artistRepository.GetByIdAsync(id);
        if (artist == null)
            return NotFound();
        return View(artist);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Artist artist)
    {
        if (!ModelState.IsValid)
        {
            await artistRepository.UpdateAsync(artist);
            return RedirectToAction(nameof(Index));
        }

        return View(artist);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var artist = await artistRepository.GetByIdAsync(id);
        if (artist == null)
            return NotFound();
        return View(artist);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await artistRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var artist = await artistRepository.GetByIdWithAlbumsAndSongsAsync(id);
        if (artist == null)
            return NotFound();

        var songs = artist.Albums
            .Where(a => a.Songs != null)
            .SelectMany(a => a.Songs)
            .ToList();

        var random = new Random();
        var randomSongs = songs.OrderBy(s => random.Next()).Take(5).ToList();

        ViewBag.RandomSongs = randomSongs;

        return View(artist);
    }
}