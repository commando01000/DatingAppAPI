using Common.Layer;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using Services.Layer.DTOs.Account;

namespace Services.Layer.Identity
{
    public interface IAccountService
    {
        public Task<Response<PaginatedResultDTO<UserDTO>>> GetUsersWithSpecs(UserSpecifications userSpecifications);
        public Task<Response<UserDTO>> RegisterUser(RegisterDTO userDTO);
        public string GetCurrentUserId();
        public string GetCurrentUserEmail();
        public string GetCurrentUsername();
        public Task<Response<Nothing>> LoginUser(LoginDTO userDTO);
    }
}
