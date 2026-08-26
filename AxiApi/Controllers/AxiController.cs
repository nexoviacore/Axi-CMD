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

        public AxiController(
            IGrammarService grammarService,
            IUserFavouriteService userFavouriteService,
            ICommandConfigService commandConfigService
        )
        {
            _grammarService = grammarService;
            _userFavouriteService = userFavouriteService;
            _commandConfigService = commandConfigService;
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

        [HttpGet("command-config/all")]
        public async Task<ActionResult<List<CommandConfigDTO>>> GetAllCommandConfigs([FromQuery] string appname)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");

            var configs = await _commandConfigService.GetAllCommandConfigsAsync(appname);
            return Ok(configs);
        }

        [HttpPost("command-config/save")]
        public async Task<ActionResult<bool>> SaveCommandConfig([FromBody] CommandConfigDTO config, [FromQuery] string appname)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");
            if (config == null || string.IsNullOrWhiteSpace(config.ConfigId))
                return BadRequest("Valid config payload with ConfigId is required.");

            var success = await _commandConfigService.SaveCommandConfigAsync(config, appname);
            return Ok(new { success = success, message = "Command configuration saved successfully." });
        }

        [HttpPost("command-config/delete")]
        public async Task<ActionResult<bool>> DeleteCommandConfig([FromQuery] string configId, [FromQuery] string appname)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");
            if (string.IsNullOrWhiteSpace(configId))
                return BadRequest("configId query parameter is required.");

            var success = await _commandConfigService.DeleteCommandConfigAsync(configId, appname);
            return Ok(new { success = success, message = "Command configuration deleted successfully." });
        }

        [HttpGet("command-prompts/all")]
        public async Task<ActionResult<List<CommandPromptDTO>>> GetCommandPrompts([FromQuery] string appname)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");

            var prompts = await _commandConfigService.GetCommandPromptsAsync(appname);
            return Ok(prompts);
        }

        [HttpPost("command-prompts/save")]
        public async Task<ActionResult<bool>> SaveCommandPrompt([FromBody] SavePromptRequestDTO request, [FromQuery] string appname)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");
            if (request == null || request.CmdToken <= 0 || request.WordPos <= 0)
                return BadRequest("Valid prompt payload with CmdToken and WordPos is required.");

            var success = await _commandConfigService.SaveCommandPromptAsync(request, appname);
            return Ok(new { success = success, message = "Command prompt saved successfully." });
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
       
        


    }
}
