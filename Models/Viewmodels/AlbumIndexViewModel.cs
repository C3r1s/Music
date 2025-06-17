namespace Music.Models.Viewmodels;

public class AlbumIndexViewModel
{
    public List<Album> Albums { get; set; } = [];
    public PaginationViewModel Pagination { get; set; } = new();
    public HashSet<int> FavouriteAlbumIds { get; set; } = [];
}