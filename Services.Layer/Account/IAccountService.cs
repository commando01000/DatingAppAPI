using Common.Layer;
using Services.Layer.DTOs;

namespace Services.Layer.Identity
{
    public interface IAccountService
    {
        public Task<Response<IEnumerable<UserDTO>>> GetAllUsers();
        public Task<Response<UserDTO>> GetUserById(Guid id);
    }
}
