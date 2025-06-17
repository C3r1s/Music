using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Models;
using Music.Models.Viewmodels;

namespace Music.Controllers;

[Authorize]
public class ArtistController(
    IArtistRepository artistRepository,
    IFileRepository fileRepository) : Controller
{
    private const int PageSize = 4;


    public async Task<IActionResult> Index(int page = 1, string query = "")
    {
        var totalItems = await artistRepository.GetCountByQueryAsync(query);
        var artistsOnPage = await artistRepository.GetAllByQueryAsync(query, (page - 1) * PageSize, PageSize);

        var model = new ArtistIndexViewModel
        {
            Artists = artistsOnPage,
            Pagination = new PaginationViewModel
            {
                PageNumber = page,
                PageSize = PageSize,
                TotalItems = totalItems
            },
            Query = query
        };
        return View(model);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateArtistViewModel createArtistViewModel)
    {
        if (ModelState.IsValid)
        {
            var urlImg = await fileRepository.UploadFileAsync(createArtistViewModel.File);
            var artist = new Artist
            {
                Name = createArtistViewModel.Name,
                UrlImg = urlImg,
                Albums = new List<Album>()
            };
            await artistRepository.AddAsync(artist);
            return RedirectToAction(nameof(Index));
        }

        return View();
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

    [HttpPost]
    [ActionName("Delete")]
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
        var randomSongs = new HashSet<Song>();

        var shuffledSongs = songs.OrderBy(s => random.Next()).ToList();

        foreach (var song in shuffledSongs.Take(5)) randomSongs.Add(song);

        ViewBag.RandomSongs = randomSongs;
        return View(artist);
    }
}