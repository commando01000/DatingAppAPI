﻿using Common.Layer;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using Services.Layer.DTOs.Account;

namespace Services.Layer.Identity
{
    public interface IAccountService
    {
        public Task<Response<MemberDTO>> RegisterUser(RegisterDTO userDTO);
        public Task<string> GetCurrentUserId();
        public Task<string> GetCurrentUserEmail();
        public Task<string> GetCurrentUsername();
        public Task<Response<Nothing>> LoginUser(LoginDTO userDTO);
    }
}
