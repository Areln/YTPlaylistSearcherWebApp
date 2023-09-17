using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

    [ApiController]
    [Route("[controller]")]
    public class EnvironmentController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public EnvironmentController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [HttpGet("routes")]
        [Produces(typeof(ListResult<RouteModel>))]
        public IActionResult GetAllRoutes()
        {

            var result = new ListResult<RouteModel>();
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(
                ad => ad.AttributeRouteInfo != null).Select(ad => new RouteModel
                {
                    Name = ad.AttributeRouteInfo.Name,
                    Template = ad.AttributeRouteInfo.Template
                }).ToList();
            if (routes != null && routes.Any())
            {
                result.Items = routes;
                result.Success = true;
            }
            return Ok(result);
        }
    }

    internal class RouteModel
    {
        public string Name { get; set; }
        public string Template { get; set; }
    }


    internal class ListResult<T>
    {
        public ListResult()
        {
        }

        public List<RouteModel> Items { get; internal set; }
        public bool Success { get; internal set; }
    }
}
