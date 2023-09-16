using Microsoft.EntityFrameworkCore;
using YTPlaylistSearcherWebApp.Data.CS;
using YTPlaylistSearcherWebApp.Models.CS;

namespace YTPlaylistSearcherWebApp.Repositories
{
    public class LineUpsRepository : ILineUpsRepository
    {

        public LineUpsRepository()
        {

        }

        public async Task<IEnumerable<Lineup>> GetLineUps(CSContext context)
        {
            return context.Lineups
                .Include(x => x.From)
                .Include(x => x.To)
                .Include(x => x.Map)
                .Include(x => x.GrenadeType)
                .Include(x => x.Path)
                .Include(x => x.Team)
                .Include(x => x.ThrowStyleType)
                .AsEnumerable();
        }
    }

    public interface ILineUpsRepository
    {
        Task<IEnumerable<Lineup>> GetLineUps(CSContext context);
    }
}
