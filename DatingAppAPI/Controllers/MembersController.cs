using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using Services.Layer.Member;
using Services.Layer;
using Common.Layer;

namespace DatingAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IPhotoService _photoService;
        public MembersController(IMemberService memberService, IPhotoService photoService)
        {
            _memberService = memberService;
            _photoService = photoService;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> members([FromQuery] MemberSpecifications spec)
        {
            var members = await _memberService.GetMembersWithSpecs(spec);
            return Ok(members);
        }

        [HttpPut]
        //[Authorize]
        public async Task<IActionResult> update(MemberDTO memberDTO)
        {
            var result = await _memberService.UpdateMember(memberDTO);
            return Ok(result);
        }

        // add photo service
        [HttpPost]
        public async Task<ActionResult<Response<PhotoDTO>>> AddPhoto(IFormFile file)
        {
            var result = await _photoService.AddPhotoAsync(file);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Response<PhotoDTO>>> SetMainPhoto(int photoId)
        {
            var result = await _photoService.SetMainPhoto(photoId);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult<Response<PhotoDTO>>> DeletePhoto(string publicId)
        {
            var result = await _photoService.DeletePhotoAsync(publicId);
            return Ok(result);
        }
    }
}
