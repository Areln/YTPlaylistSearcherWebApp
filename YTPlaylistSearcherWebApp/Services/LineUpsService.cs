using YTPlaylistSearcherWebApp.Data.CS;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Repositories;

namespace YTPlaylistSearcherWebApp.Services
{
    public class LineUpsService : ILineUpsService
    {
        private readonly ILineUpsRepository _lineUpsRepository;

        public LineUpsService(ILineUpsRepository lineUpsRepository)
        {
            _lineUpsRepository = lineUpsRepository;
        }

        public async Task<IEnumerable<LineUpDTO>> GetLineUps(CSContext context)
        {
            var results = await _lineUpsRepository.GetLineUps(context);
            return results.Select(x => new LineUpDTO
            {
                Id = x.Id,
                Path = x.Path,
                GrenadeType = x.GrenadeType.Name,
                ThrowStyle = x.ThrowStyleType.Name,
                Map = x.Map.Name,
                MapPath = "TODO: add column for the map picture",
                Team = x.Team.Name,
                TeamAbbreviation = x.Team.Abbreviation,
                From = x.From,
                To = x.To,
            });
        }
    }

    public interface ILineUpsService
    {
        Task<IEnumerable<LineUpDTO>> GetLineUps(CSContext context);
    }
}
