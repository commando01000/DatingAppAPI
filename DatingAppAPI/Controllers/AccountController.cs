using Common.Layer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using Services.Layer.DTOs.Account;
using Services.Layer.Identity;

namespace DatingAppAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> users([FromQuery] UserSpecifications spec)
        {
            var users = await _accountService.GetUsersWithSpecs(spec);
            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAuthenticatedUserId()
        {
            var Id = await _accountService.GetCurrentUserId();
            return Ok(Id);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAuthenticatedUserName()
        {
            var name = await _accountService.GetCurrentUsername();
            return Ok(name);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserEmail()
        {
            var email = await _accountService.GetCurrentUserEmail();
            return Ok(email);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> register([FromBody] RegisterDTO registerDto)
        {
            var result = await _accountService.RegisterUser(registerDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> login([FromBody] LoginDTO loginDTO)
        {
            var result = await _accountService.LoginUser(loginDTO);
            return Ok(result);
        }
    }
}
