namespace Music.Models;

public class SearchViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<Artist> Artists { get; set; } = [];
    public List<Album> Albums { get; set; } = [];
    public List<Song> Songs { get; set; } = [];
}
