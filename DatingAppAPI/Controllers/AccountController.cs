using Common.Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        
        [HttpGet]
        public async Task<IActionResult> users()
        {
            var users = await _accountService.GetAllUsers();
            return Ok(users);
        }

        // Get User By Id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _accountService.GetUserById(id);
            return Ok(user);
        }
    }
}
