namespace Music.Models.Viewmodels;

public class FavouritesViewModel
{
    public List<Artist> Artists { get; set; } = [];
    public List<Album> Albums { get; set; } = [];
    public List<Song> Songs { get; set; } = [];
}