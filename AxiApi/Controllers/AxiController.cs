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
        public async Task<ActionResult<List<CommandConfigDTO>>> GetCommandConfigs([FromQuery] string appname, [FromQuery] bool forceRefresh = false)
        {
            if (string.IsNullOrWhiteSpace(appname))
                return BadRequest("appname query parameter is required.");

            var configs = await _commandConfigService.GetCommandConfigsAsync(appname, forceRefresh);
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
       
        


    }
}
