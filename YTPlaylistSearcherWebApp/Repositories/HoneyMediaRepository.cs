using Microsoft.EntityFrameworkCore;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.Models;

namespace YTPlaylistSearcherWebApp.Repositories
{
    public class HoneyMediaRepository : IHoneyMediaRepository
    {
        public async Task<IEnumerable<Honeymedia>> GetAllHoneyMedia(YTPSContext context)
        {
            return await context.Honeymedia
                .Include(x => x.MediaType)
                .Include(x => x.InterestType)
                .Include(x => x.RequestingUser)
                .Include(x => x.Response)
                .Include(x => x.Status)
                .OrderByDescending(x => x.LastUpdated ?? x.DateRequested ?? DateTime.MinValue)
                .ToListAsync();
        }

        public async Task<Honeymedia?> GetHoneyMediaById(YTPSContext context, int id)
        {
            return await context.Honeymedia
                .Include(x => x.MediaType)
                .Include(x => x.InterestType)
                .Include(x => x.RequestingUser)
                .Include(x => x.Response)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddHoneyMedia(YTPSContext context, Honeymedia honeyMedia)
        {
            honeyMedia.LastUpdated = DateTime.UtcNow;
            if (honeyMedia.DateRequested == null)
            {
                honeyMedia.DateRequested = DateTime.UtcNow;
            }
            await context.Honeymedia.AddAsync(honeyMedia);
        }

        public async Task UpdateHoneyMedia(YTPSContext context, Honeymedia honeyMedia)
        {
            honeyMedia.LastUpdated = DateTime.UtcNow;
            context.Honeymedia.Update(honeyMedia);
        }

        public async Task DeleteHoneyMedia(YTPSContext context, Honeymedia honeyMedia)
        {
            context.Honeymedia.Remove(honeyMedia);
        }

        public async Task<IEnumerable<Mediatype>> GetMediaTypes(YTPSContext context)
        {
            return await context.Mediatypes.ToListAsync();
        }

        public async Task<IEnumerable<Mediainterest>> GetMediaInterests(YTPSContext context)
        {
            return await context.Mediainterests.ToListAsync();
        }

        public async Task<IEnumerable<Mediarequester>> GetMediaRequesters(YTPSContext context)
        {
            return await context.Mediarequesters.ToListAsync();
        }

        public async Task<IEnumerable<Mediaresponse>> GetMediaResponses(YTPSContext context)
        {
            return await context.Mediaresponses.ToListAsync();
        }

        public async Task<IEnumerable<Mediastatus>> GetMediaStatuses(YTPSContext context)
        {
            return await context.Mediastatuses.ToListAsync();
        }
    }

    public interface IHoneyMediaRepository
    {
        Task<IEnumerable<Honeymedia>> GetAllHoneyMedia(YTPSContext context);
        Task<Honeymedia?> GetHoneyMediaById(YTPSContext context, int id);
        Task AddHoneyMedia(YTPSContext context, Honeymedia honeyMedia);
        Task UpdateHoneyMedia(YTPSContext context, Honeymedia honeyMedia);
        Task DeleteHoneyMedia(YTPSContext context, Honeymedia honeyMedia);
        Task<IEnumerable<Mediatype>> GetMediaTypes(YTPSContext context);
        Task<IEnumerable<Mediainterest>> GetMediaInterests(YTPSContext context);
        Task<IEnumerable<Mediarequester>> GetMediaRequesters(YTPSContext context);
        Task<IEnumerable<Mediaresponse>> GetMediaResponses(YTPSContext context);
        Task<IEnumerable<Mediastatus>> GetMediaStatuses(YTPSContext context);
    }
}

