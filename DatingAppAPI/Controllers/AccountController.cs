using Common.Layer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> users()
        {
            var users = await _accountService.GetAllUsers();
            return Ok(users);
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

        // Get User By Id
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> user(Guid id)
        {
            var user = await _accountService.GetUserById(id);
            return Ok(user);
        }
    }
}
