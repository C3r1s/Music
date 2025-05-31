namespace Music.Models;

public class Artist
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string UrlImg { get; set; }
    public required List<Album> Albums { get; set; }
}