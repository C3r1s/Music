using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;

namespace Music.Controllers;

public class AlbumController(IAlbumRepository albumRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var albums = await albumRepository.GetAllAsync();

        return View(albums);
    }

    public async Task<IActionResult> Details(int id, string name)
    {
        var album = await albumRepository.GetDetailsByIdAsync(id);

        return View(album);
    }
}