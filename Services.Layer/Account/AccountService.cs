using AutoMapper;
using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Layer.Interfaces;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using Services.Layer.DTOs.Account;
using Services.Layer.Identity;
using Services.Layer.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value;
            return userId;
        }

        public async Task<string> GetCurrentUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public async Task<string> GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        public async Task<Response<Nothing>> LoginUser(LoginDTO userDTO)
        {
            if (userDTO.Email == null || userDTO.Password == null)
            {
                return new Response<Nothing>()
                {
                    Data = null,
                    Message = "Email or Password is null",
                    Status = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    RedirectURL = null
                };
            }

            var user = await _userManager.FindByEmailAsync(userDTO.Email);

            if (user == null)
            {
                return new Response<Nothing>()
                {
                    Data = null,
                    Message = "User does not exist",
                    Status = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    RedirectURL = null
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, userDTO.Password);

            if (result)
            {
                // Generate JWT Token
                var token = await _tokenService.GenerateAccessToken(user);

                return new Response<Nothing>()
                {
                    Data = null,
                    Message = "Success",
                    Status = true,
                    Token = token,
                    StatusCode = (int)HttpStatusCode.OK,
                    RedirectURL = null
                    //RedirectURL = $"http://localhost:4200/external-login-callback?token={token}" // TODO: Change this in external login
                };
            }

            return new Response<Nothing>()
            {
                Data = null,
                Message = "Password is incorrect",
                Status = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                RedirectURL = null
            };
        }

        public async Task<Response<MemberDTO>> RegisterUser(RegisterDTO userDTO)
        {
            var isUserExists = await _userManager.FindByEmailAsync(userDTO.Email);
            if (userDTO.Password == userDTO.RePassword && isUserExists == null) // check if passwords match and user does not exist
            {
                var user = new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userDTO.Username,
                    Email = userDTO.Email,
                    Bio = userDTO.Bio,
                    DisplayName = userDTO.DisplayName == null ? userDTO.Username : userDTO.DisplayName,
                    Address = userDTO.Address == null ? null : new Address()
                    {
                        City = userDTO.Address.City,
                        Street = userDTO.Address.Street,
                        Id = Guid.NewGuid(),
                        State = userDTO.Address.State,
                        ZipCode = userDTO.Address.ZipCode
                    }
                };

                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    return new Response<MemberDTO>()
                    {
                        Data = null,
                        Message = "User could not be created",
                        Status = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        RedirectURL = null
                    };
                }

                var response = new Response<MemberDTO>()
                {
                    Data = new MemberDTO()
                    {
                        Id = user.Id,
                        Token = await _tokenService.GenerateAccessToken(user),
                    },
                    Message = "Success",
                    Status = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    RedirectURL = null,
                };
                return response;
            }
            else
            {
                return new Response<MemberDTO>()
                {
                    Data = null,
                    Message = "Passwords do not match",
                    Status = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    RedirectURL = null
                };
            }
        }
    }
}