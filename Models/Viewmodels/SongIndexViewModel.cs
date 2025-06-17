namespace Music.Models.Viewmodels;

public class SongIndexViewModel
{
    public required Album Album { get; set; }
    public required PaginationViewModel Pagination { get; set; }
}