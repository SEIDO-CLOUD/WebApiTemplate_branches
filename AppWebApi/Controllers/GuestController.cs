using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Models.DTO;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GuestController : Controller
    {
        readonly IZooService _service = null;
        readonly ILogger<GuestController> _logger = null;

        public GuestController(IZooService service, ILogger<GuestController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<GstUsrInfoAllDto>))]
        public async Task<IActionResult> Info()
        {
            try
            {
                var info = await _service.InfoAsync();

                _logger.LogInformation($"{nameof(Info)}:\n{JsonConvert.SerializeObject(info)}");
                return Ok(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Info)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

