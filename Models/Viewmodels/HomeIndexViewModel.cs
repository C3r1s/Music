namespace Music.Models.Viewmodels;

public class HomeIndexViewModel
{
    public List<Artist> Artists { get; set; } = [];
    public HashSet<int> FavouriteArtistIds { get; set; } = [];
}