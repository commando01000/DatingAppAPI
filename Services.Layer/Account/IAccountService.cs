using Common.Layer;
using Data.Layer.Entities.Identity;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using Services.Layer.DTOs.Account;

namespace Services.Layer.Identity
{
    public interface IAccountService
    {
        public Task<Response<Nothing>> LoginUser(LoginDTO userDTO);
        public Task<Response<MemberDTO>> RegisterUser(RegisterDTO userDTO);
        public string? GetCurrentUserId();
        public Task<AppUser?> GetCurrentUserAsync();
        public Task<string?> GetCurrentUserDisplayName();
        public Task<string?> GetCurrentUserEmail();
        public Task<string?> GetCurrentUserRole();
    }
}
