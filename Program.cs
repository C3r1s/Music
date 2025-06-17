using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Music;
using Music.Data.Repositories;
using Music.Data.Repositories.Interfaces;
using Uploadcare;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<UploadcareKeys>(builder.Configuration.GetSection("UploadcareKeys"));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });
builder.Services.AddAuthorization();

var connection = builder.Configuration.GetConnectionString("MusicDbConnection");

builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddSingleton<UploadcareClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var publicKey = configuration["Uploadcare:PublicKey"];
    var secretKey = configuration["Uploadcare:SecretKey"];

    return new UploadcareClient(publicKey, secretKey);
});


builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseAuthentication();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.UseAuthorization();
app.Run();

public static class PagedListQueryableExtensions
{
    public static async Task<List<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize)
    {
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return items;
    }
}