using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.Data.CS;
using YTPlaylistSearcherWebApp.Repositories;
using YTPlaylistSearcherWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7298",
            ValidAudience = "https://localhost:7298",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Secret"))) // TODO: change this
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:44422", "https://localhost:44422", "https://localhost:7298")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddSignalR();

builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddTransient<IPlaylistService, PlaylistService>();
builder.Services.AddTransient<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddTransient<ILineUpsService, LineUpsService>();
builder.Services.AddTransient<ILineUpsRepository, LineUpsRepository>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<ILoginRepository, LoginRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddDbContext<YTPSContext>(options => options.UseMySql(builder.Configuration.GetValue<string>("ConnectionStringYTPS"), ServerVersion.Create(1, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql)));
builder.Services.AddDbContext<CSContext>(options => options.UseMySql(builder.Configuration.GetValue<string>("ConnectionStringCS"), ServerVersion.Create(1, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql)));

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
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.MapHub<ShareFeedHub>("/posts");

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
