using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories;
using Music.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthorization();
app.Run();