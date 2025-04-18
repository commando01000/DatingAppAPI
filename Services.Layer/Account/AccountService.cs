using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Repository.Layer.Interfaces;
using Services.Layer.DTOs;
using Services.Layer.Identity;
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
        public AccountService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
    }
}
