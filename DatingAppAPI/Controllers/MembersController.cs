using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using Services.Layer.Member;

namespace DatingAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
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
    }
}
