using AxiApi.DTOs;
using AxiApi.Enums;
using AxiApi.Interfaces;
using AxiApi.Services;
using ARMCommon.ActionFilter;
using ARMCommon.Filter;
using ARMCommon.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using ARMCommon.Model;

namespace AxiApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class AxiController : ControllerBase
    {
        private readonly IGrammarService _grammarService;
        private readonly IUserFavouriteService _userFavouriteService;
        private readonly ICommandConfigService _commandConfigService;
        private readonly IKeyfieldService _keyfieldService;
        private readonly IPluginUninstallService _pluginUninstallService;

        public AxiController(
            IGrammarService grammarService,
            IUserFavouriteService userFavouriteService,
            ICommandConfigService commandConfigService,
            IKeyfieldService keyfieldService,
            IPluginUninstallService pluginUninstallService
        )
        {
            _grammarService = grammarService;
            _userFavouriteService = userFavouriteService;
            _commandConfigService = commandConfigService;
            _keyfieldService = keyfieldService;
            _pluginUninstallService = pluginUninstallService;
        }

        [HttpGet("command-config")]
        public async Task<ActionResult<List<CommandConfigDTO>>> GetCommandConfigs([FromQuery] string appname, [FromQuery] string username, [FromQuery] bool forceRefresh = false)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("username query parameter is required.");

            var configs = await _commandConfigService.GetCommandConfigsAsync(appname, username, forceRefresh);
            return Ok(configs);
        }

        [HttpGet("axi_get")]

        public async Task<IActionResult> AxiGet([FromQuery] string view, [FromQuery] bool forceRefresh, [FromQuery] string appname)
        {

           
                if (!Enum.TryParse<GrammarView>(view, true, out var parsedView))
                    return BadRequest($"Invalid view: {parsedView}");
                var commands = await _grammarService.Get(parsedView, forceRefresh, appname);
                return Ok(commands);
            
            
        }

        [HttpGet("user-favourites")]
        public async Task<ActionResult<List<UserFavouritesDTO>>> getUserFavouritesByUser([FromQuery] string username, [FromQuery] string appname)
        {
            List<UserFavouritesDTO> userFavourites = await _userFavouriteService.GetUserFavouritesByUsernameAsync(username, appname);

            return Ok(userFavourites); 

        }

        [HttpPost("user-favourites")]
        public async Task<ActionResult<object>> ToggleUserFavourites([FromBody] UserFavouritesRequestDTO requestDTO, [FromQuery] string appname)

        {
            object response = await _userFavouriteService.ToggleUserFavouritesAsync(requestDTO, appname);
            return Ok(response); 

        }

        [HttpPatch("user-favourites/{favouritesId}")]
        public async Task<ActionResult<NonQueryResult>> UpdateCommandText([FromBody] UpdateUserFavouritesDTO requestDTO, [FromQuery] string appname, [FromQuery] string username, [FromRoute] string favouritesId)
        {
            NonQueryResult response = await _userFavouriteService.UpdateCommandText(favouritesId, requestDTO, appname, username);
            return Ok(response); 
        }

        [HttpPost("setkeyfield")]
        public async Task<ActionResult<ApiResponseDTO>> SetKeyfield([FromBody] SetKeyfieldRequestDTO requestDTO)
        {
            if (string.IsNullOrWhiteSpace(requestDTO.AppName))
                return BadRequest("AppName is required in payload.");

            var response = await _keyfieldService.SetKeyfieldAsync(requestDTO);
            return Ok(response);
        }

        [HttpGet("plugins")]
        public ActionResult<IReadOnlyList<PluginDTO>> GetInstalledPlugins([FromQuery] string appname, [FromQuery] bool configureAccess)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");

            if (!configureAccess)
                return StatusCode(StatusCodes.Status403Forbidden, "Configure administration access is required to uninstall Axi CMD.");

            return Ok(_pluginUninstallService.GetInstalledPlugins());
        }

        [HttpDelete("plugins/{pluginId}")]
        public async Task<ActionResult<ApiResponseDTO>> UninstallPlugin([FromRoute] string pluginId, [FromQuery] string appname, [FromQuery] bool configureAccess)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");
            if (!pluginId.Equals("Axi_Beta_2", StringComparison.OrdinalIgnoreCase))
                return NotFound(new ApiResponseDTO { Success = false, Message = "Plugin not found.", StatusCode = StatusCodes.Status404NotFound });
            if (!configureAccess)
                return StatusCode(StatusCodes.Status403Forbidden, new ApiResponseDTO { Success = false, Message = "Configure administration access is required to uninstall Axi CMD.", StatusCode = StatusCodes.Status403Forbidden });

            var response = await _pluginUninstallService.UninstallAxiCmdAsync();
            return Ok(response);
        }
    }
}
