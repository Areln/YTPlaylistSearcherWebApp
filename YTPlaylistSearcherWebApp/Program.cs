using Microsoft.EntityFrameworkCore;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.Repositories;
using YTPlaylistSearcherWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddTransient<IPlaylistService, PlaylistService>();
builder.Services.AddTransient<IPlaylistRepository, PlaylistRepository>();

builder.Services.AddDbContext<YTPSContext>(options => options.UseMySql(builder.Configuration.GetValue<string>("ConnectionString"), ServerVersion.Create(1, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

if (app.Environment.IsDevelopment())
{
    app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
}

app.MapFallbackToFile("index.html"); ;

app.Run();
