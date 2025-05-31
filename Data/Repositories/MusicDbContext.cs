using Microsoft.EntityFrameworkCore;
using Music.Models;

namespace Music.Data.Repositories;

public class MusicDbContext(DbContextOptions<MusicDbContext> options) : DbContext(options)
{
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }
}