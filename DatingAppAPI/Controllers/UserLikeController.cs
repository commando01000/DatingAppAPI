using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Layer.Specifications.UserLikes;
using Services.Layer.DTOs;
using Services.Layer.Identity;
using Services.Layer.UserLikes;

namespace DatingAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserLikeController : ControllerBase
    {
        private readonly IUserLikeService _userLikeService;
        private readonly IAccountService _accountService;

        public UserLikeController(IUserLikeService userLikeService, IAccountService accountService)
        {
            _userLikeService = userLikeService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> LikeUser(UserLikeSpecifications userLikeSpecifications)
        {
            var SourceUserId = _accountService.GetCurrentUserId();

            if (SourceUserId == userLikeSpecifications.SourceUserId)
            {
                return BadRequest("You cannot like yourself");
            }

            userLikeSpecifications.SourceUserId = SourceUserId;
            var result = await _userLikeService.GetUserLike(userLikeSpecifications);

            if (result == null)
            {
                var addLikeDto = new UserLikeDTO { SourceUserId = SourceUserId, LikedUserId = userLikeSpecifications.LikedUserId };
                var res = await _userLikeService.AddLike(addLikeDto);
                return Ok(res);
            }
            else
            {
                var deleteLikeDto = new UserLikeDTO { SourceUserId = SourceUserId, LikedUserId = userLikeSpecifications.LikedUserId };
                var res = await _userLikeService.RemoveLike(deleteLikeDto);
                return Ok(res);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetCurrentUserLikes([FromQuery] UserLikeSpecifications userLikeSpecifications)
        {
            var SourceUserId = _accountService.GetCurrentUserId();
            userLikeSpecifications.SourceUserId = SourceUserId;
            var result = await _userLikeService.GetCurrentUserLikeIds(userLikeSpecifications);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUserLikes([FromQuery] UserLikeSpecifications userLikeSpecifications)
        {
            userLikeSpecifications.SourceUserId = _accountService.GetCurrentUserId();
            userLikeSpecifications.userId = userLikeSpecifications.SourceUserId;
            var result = await _userLikeService.GetUserLikes(userLikeSpecifications);
            return Ok(result);
        }
    }
}
