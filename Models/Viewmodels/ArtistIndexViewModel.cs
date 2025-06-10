namespace Music.Models.Viewmodels;

public class ArtistIndexViewModel
{
    public List<Artist> Artists { get; set; }
    public PaginationViewModel Pagination { get; set; }
    public string Query { get; set; }
}
