using Microsoft.AspNetCore.Mvc;
using Music.Data.Repositories.Interfaces;
using Music.Models;

namespace Music.Controllers;

public class ArtistController(IArtistRepository artistRepository) : Controller
{
    // Отображение списка артистов
    public async Task<IActionResult> Index()
    {
        var artists = await artistRepository.GetAllAsync();
        return View(artists);
    }

    // Форма создания артиста
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Artist artist)
    {
        if (ModelState.IsValid)
        {
            await artistRepository.AddAsync(artist);
            return RedirectToAction(nameof(Index));
        }
        return View(artist);
    }

    // Форма редактирования артиста
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
        if (ModelState.IsValid)
        {
            await artistRepository.UpdateAsync(artist);
            return RedirectToAction(nameof(Index));
        }
        return View(artist);
    }

    // Удаление артиста
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
}
