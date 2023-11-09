using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tw_Clone.Dto.User;
using Tw_Clone.Services;

namespace Tw_Clone.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController( UserService userService) { 
          _userService = userService;
        }


        [HttpGet("username/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get(string username)
        {
            try
            {
                return Ok(await _userService.GetUserByUsername(username));
            }
            catch
            {
                return NotFound(new { message = $"No user with UserName = {username}" });
            }
        }


        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VerifyUserDto>> GetUser(string email)
        {
            try
            {
                return Ok(await _userService.GetUserByEmail(email));
            }
            catch
            {
                return NotFound(new { message = $"No user with Email = {email}" });
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Post([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userCreated = await _userService.Create(createUserDto);
            return Created("CreateUser", userCreated);

        }



        [HttpPut("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Put(string username, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var userUpdated = await _userService.UpdateByUsername(username, updateUserDto);
                return Ok(userUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(string username)
        {
            try
            {
                await _userService.Delete(username);
                // Se puede retornar un No content (204)
                return Ok(new
                {
                    message = $"User with Id = {username} was deleted"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
