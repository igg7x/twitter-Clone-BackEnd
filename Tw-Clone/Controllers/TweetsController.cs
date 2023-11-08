using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tw_Clone.Dto.Tweet;
using Tw_Clone.Dto.TweetsLike;
using Tw_Clone.Dto.TweetsReposts;
using Tw_Clone.Dto.User;
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
                return NotFound(new { message = $"No tweet with Id = {id}" });
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
            UserDto user; 
            try
            {
                user =  await _userService.GetUserByUsername(createTweetDto.UserName);
            }
            catch
            {
                ModelState.AddModelError("UserName", "User does not exist");
                return BadRequest(ModelState);
            }
            var postCreated = await _tweetService.Create(createTweetDto , user.Id);
            return Created("CreatePost", postCreated);
        }

        [HttpPost("comment/{tweetid:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TweetDto>> PostComment(int tweetid ,  [FromBody] CreateTweetDto createTweetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserDto user;
            TweetDto tweet;
            try
            {
                user = await _userService.GetUserByUsername(createTweetDto.UserName);
            }
            catch
            {
                ModelState.AddModelError("UserName", "User does not exist");
                return BadRequest(ModelState);
            }

            try
            {
                tweet = await _tweetService.GetById(tweetid);
            }
            catch
            {
                ModelState.AddModelError("Tweet", "Tweet does not exist");
                return BadRequest(ModelState);
            }
           

           

            var commentCreated = await _tweetService.Create(createTweetDto, user.Id);
            await _tweetService.CreateComment(tweet.Id, commentCreated ,commentCreated.Id); 

            return Created("CreateComment to tweet",commentCreated);
        }

        [HttpPost("like")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostLike([FromBody] CreateTweetLikeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDto user;
            try
            {
                user = await _userService.GetUserByUsername(dto.UserName);
            }
            catch
            {
                ModelState.AddModelError("UserName", "User does not exist");
                return BadRequest(ModelState);
            }

            if (_tweetService.GetLikeIfExists(dto.TweetId, user.Id))
            {
                await _tweetService.DeleteLikeToTweet(dto, user.Id);
                return Ok();
            }
            else {
                await _tweetService.AddLikeToTweet(dto, user.Id);
                return Ok();
            }

        }

        [HttpPost("Repost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> PostRepost([FromBody] CreateTweetRepostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDto user;
            try
            {
                user = await _userService.GetUserByUsername(dto.UserName);
            }
            catch
            {
                ModelState.AddModelError("UserName", "User does not exist");
                return BadRequest(ModelState);
            }

            if (_tweetService.GetRepostsIfExists(dto.TweetId, user.Id))
            {
                await _tweetService.DeleteRepostToTweet(dto, user.Id);
                return Ok();
            }
            else
            {
                await _tweetService.AddRepostToTweet(dto, user.Id);
                return Ok();
            }
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
