using Common.Layer;
using Services.Layer.DTOs;
using Services.Layer.DTOs.Account;

namespace Services.Layer.Identity
{
    public interface IAccountService
    {
        public Task<Response<IEnumerable<UserDTO>>> GetAllUsers();
        public Task<Response<UserDTO>> GetUserById(Guid id);
        public Task<Response<UserDTO>> RegisterUser(RegisterDTO userDTO);
        public Task<Response<Nothing>> LoginUser(LoginDTO userDTO);
    }
}
