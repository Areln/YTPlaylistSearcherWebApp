using Microsoft.AspNetCore.Mvc;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Services;

namespace YTPlaylistSearcherWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HoneyMediaController : ControllerBase
    {
        private readonly ILogger<HoneyMediaController> _logger;
        private readonly YTPSContext _context;
        private readonly IHoneyMediaService _honeyMediaService;

        public HoneyMediaController(ILogger<HoneyMediaController> logger, YTPSContext context, IHoneyMediaService honeyMediaService)
        {
            _logger = logger;
            _context = context;
            _honeyMediaService = honeyMediaService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _honeyMediaService.GetAllHoneyMedia(_context);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetAll");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _honeyMediaService.GetHoneyMediaById(_context, id);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetById");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] HoneyMediaDTO dto)
        {
            try
            {
                var result = await _honeyMediaService.CreateHoneyMedia(_context, dto);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Create");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HoneyMediaDTO dto)
        {
            try
            {
                var result = await _honeyMediaService.UpdateHoneyMedia(_context, id, dto);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Update");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _honeyMediaService.DeleteHoneyMedia(_context, id);
                if (!result)
                    return NotFound();
                return Ok(new { success = true });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Delete");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpGet("GetMediaTypes")]
        public async Task<IActionResult> GetMediaTypes()
        {
            try
            {
                var result = await _honeyMediaService.GetMediaTypes(_context);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetMediaTypes");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpGet("GetMediaInterests")]
        public async Task<IActionResult> GetMediaInterests()
        {
            try
            {
                var result = await _honeyMediaService.GetMediaInterests(_context);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetMediaInterests");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpGet("GetMediaRequesters")]
        public async Task<IActionResult> GetMediaRequesters()
        {
            try
            {
                var result = await _honeyMediaService.GetMediaRequesters(_context);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetMediaRequesters");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpGet("GetMediaResponses")]
        public async Task<IActionResult> GetMediaResponses()
        {
            try
            {
                var result = await _honeyMediaService.GetMediaResponses(_context);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetMediaResponses");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }

        [HttpGet("GetMediaStatuses")]
        public async Task<IActionResult> GetMediaStatuses()
        {
            try
            {
                var result = await _honeyMediaService.GetMediaStatuses(_context);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetMediaStatuses");
                return BadRequest(e.Message + " " + e.InnerException?.Message);
            }
        }
    }
}

