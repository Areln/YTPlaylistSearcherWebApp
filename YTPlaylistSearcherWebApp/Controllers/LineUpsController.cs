using Microsoft.AspNetCore.Mvc;
using YTPlaylistSearcherWebApp.Data.CS;
using YTPlaylistSearcherWebApp.Services;

namespace YTPlaylistSearcherWebApp.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LineUpsController : ControllerBase
    {
        private readonly ILogger<LineUpsController> _logger;
        private readonly CSContext _context;
        private readonly ILineUpsService _lineUpsService;

        public LineUpsController(ILogger<LineUpsController> logger, CSContext context, ILineUpsService lineUpsService)
        {
            _logger = logger;
            _context = context;
            _lineUpsService = lineUpsService;
        }

        [HttpGet("GetLineUps")]
        public async Task<IActionResult> GetLineUps()
        {
            try
            {
                var playlist = await _lineUpsService.GetLineUps(_context);
                return Ok(playlist);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetLineUps");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }
    }
}
