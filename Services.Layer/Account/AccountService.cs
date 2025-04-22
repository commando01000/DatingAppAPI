using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Repository.Layer.Interfaces;
using Services.Layer.DTOs;
using Services.Layer.DTOs.Account;
using Services.Layer.Identity;
using Services.Layer.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<Response<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            return new Response<IEnumerable<UserDTO>>()
            {
                Data = users.Select(x => new UserDTO()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Username = x.UserName,
                    Bio = x.Bio,
                    DisplayName = x.DisplayName,
                    Address = x.Address == null ? null : new AddressDTO
                    {
                        City = x.Address.City,
                        Street = x.Address.Street,
                        Id = x.Address.Id,
                        State = x.Address.State,
                        ZipCode = x.Address.ZipCode
                    }
                }),
                Message = "Success",
                Status = true,
                StatusCode = (int)HttpStatusCode.OK,
                RedirectURL = null
            };
        }

        public async Task<Response<UserDTO>> GetUserById(Guid id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;

            return new Response<UserDTO>()
            {
                Data = new UserDTO()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Bio = user.Bio,
                    DisplayName = user.DisplayName,
                    Address = user.Address == null ? null : new AddressDTO
                    {
                        City = user.Address.City,
                        Street = user.Address.Street,
                        Id = user.Address.Id,
                        State = user.Address.State,
                        ZipCode = user.Address.ZipCode
                    }
                },
                Message = "Success",
                Status = true,
                StatusCode = (int)HttpStatusCode.OK,
                RedirectURL = null
            };
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

        public async Task<Response<UserDTO>> RegisterUser(RegisterDTO userDTO)
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
                    return new Response<UserDTO>()
                    {
                        Data = null,
                        Message = "User could not be created",
                        Status = false,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        RedirectURL = null
                    };
                }

                var response = new Response<UserDTO>()
                {
                    Data = new UserDTO()
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
                return new Response<UserDTO>()
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