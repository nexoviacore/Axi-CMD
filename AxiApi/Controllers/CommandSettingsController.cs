using AxiApi.DTOs;
using AxiApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AxiApi.Controllers
{
    /// <summary>
    /// App-wide AxiCMD command settings (axi_command_settings).
    /// Known gap: like the other Axi endpoints, nothing here checks on the server whether the caller may change settings;
    /// the client (AxiCMD) decides who sees the screen and the username in the body is trusted as sent.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class CommandSettingsController : ControllerBase
    {
        private readonly ICommandSettingsService _commandSettingsService;

        public CommandSettingsController(ICommandSettingsService commandSettingsService)
        {
            _commandSettingsService = commandSettingsService;
        }

        [HttpGet]
        public async Task<ActionResult<CommandSettingsDTO>> GetCommandSettings([FromQuery] string appname, [FromQuery] string category, [FromQuery] string? type)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest("category query parameter is required.");

            return Ok(await _commandSettingsService.GetCommandSettingsAsync(appname, category, type));
        }

        [HttpPost]
        public async Task<ActionResult<CommandSettingsSaveResultDTO>> SaveCommandSettings([FromBody] CommandSettingsRequestDTO requestDTO, [FromQuery] string appname)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");

            try
            {
                return Ok(await _commandSettingsService.SaveCommandSettingsAsync(appname, requestDTO));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
