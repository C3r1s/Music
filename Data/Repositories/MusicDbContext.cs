using Microsoft.EntityFrameworkCore;
using Music.Models;
using Music.Models.RelationModels;

namespace Music.Data.Repositories;

public class MusicDbContext(DbContextOptions<MusicDbContext> options) : DbContext(options)
{
    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserArtist> UserArtists { get; set; }
    public DbSet<UserAlbum> UserAlbums { get; set; }
    public DbSet<UserSong> UserSongs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserArtist>()
            .HasKey(ua => new { ua.UserId, ua.ArtistId });

        modelBuilder.Entity<UserAlbum>()
            .HasKey(ua => new { ua.UserId, ua.AlbumId });

        modelBuilder.Entity<UserSong>()
            .HasKey(us => new { us.UserId, us.SongId });

        modelBuilder.Entity<Artist>()
            .HasMany(a => a.Albums)
            .WithOne()
            .HasForeignKey("ArtistId") // Правильный внешний ключ
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
    }
}