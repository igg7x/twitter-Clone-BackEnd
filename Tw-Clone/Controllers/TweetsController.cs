using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tw_Clone.Dto.Tweet;
using Tw_Clone.Services;

namespace Tw_Clone.Controllers
{
    [Route("api/tweets")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly TweetService _tweetService;
        private readonly UserService _userService;


        public TweetsController(TweetService tweetService, UserService userService)
        {
            _tweetService = tweetService;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TweetsDto>>> Get()
        {
            return Ok(await _tweetService.GetAll());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TweetDto>> Get(int id)
        {
            try
            {
                return Ok(await _tweetService.GetById(id));
            }
            catch
            {
                return NotFound(new { message = $"No post with Id = {id}" });
            }
        }

         [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TweetDto>> Post([FromBody] CreateTweetDto createTweetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _userService.GetById(createTweetDto.UserId);
            }
            catch
            {
                ModelState.AddModelError("UserId", "User does not exist");
                return BadRequest(ModelState);
            }
            var postCreated = await _tweetService.Create(createTweetDto);
            return Created("CreatePost", postCreated);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TweetDto>> Put(int id, [FromBody] UpdateTweetDto updatePostDto)
        {
            try
            {
                var postUpdated = await _tweetService.UpdateById(id, updatePostDto);
                return Ok(postUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _tweetService.DeleteById(id);
                return Ok(new
                {
                    message = $"Post with Id = {id} was deleted"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
