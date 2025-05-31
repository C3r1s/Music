using Microsoft.EntityFrameworkCore;
using Music.Data.Repositories;
using Music.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("MusicDbConnection");
builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddScoped<IAlbumRepository, AlbumRepositoryAdo>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();