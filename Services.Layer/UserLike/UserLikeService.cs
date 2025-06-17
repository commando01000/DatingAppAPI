using AutoMapper;
using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Repository.Layer.Interfaces;
using Repository.Layer.Specifications.UserLikes;
using Services.Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.UserLikes
{
    public class UserLikeService : IUserLikeService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IMapper _mapper;
        public UserLikeService(UserManager<AppUser> userManager, IUnitOfWork<AppDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<Nothing>> AddLike(UserLikeDTO userLikeDTO)
        {
            var userLike = _mapper.Map<UserLike>(userLikeDTO);
            await _unitOfWork.Repository<UserLike, string>().Create(userLike);
            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
            {
                var response = new Response<Nothing>()
                {
                    Data = null,
                    Message = "Success",
                    StatusCode = 200,
                    Status = true
                };
                return response;
            }
            else
            {
                var response = new Response<Nothing>()
                {
                    Data = null,
                    Message = "Error in adding like",
                    StatusCode = 500,
                    Status = false
                };
                return response;
            }
        }

        public async Task<IEnumerable<string>> GetCurrentUserLikeIds(UserLikeSpecifications specs)
        {
            var UserLikeWithSpecs = new UserLikeWithSpecifications(specs);
            var userLikes = await _unitOfWork.Repository<UserLike, string>().GetAllWithSpecs(UserLikeWithSpecs);
            return userLikes.Select(x => x.LikedUserId).ToList();
        }

        public async Task<UserLikeDTO> GetUserLike(UserLikeSpecifications userLikeSpecifications)
        {
            var UserLikeWithSpecs = new UserLikeWithSpecifications(userLikeSpecifications);
            var GetUserLike = await _unitOfWork.Repository<UserLike, string>().GetWithSpecs(UserLikeWithSpecs);

            return _mapper.Map<UserLikeDTO>(GetUserLike);
        }

        public async Task<IEnumerable<MemberDTO>> GetUserLikes(UserLikeSpecifications userLikeSpecifications)
        {
            var UserLikeWithSpecs = new UserLikeWithSpecifications(userLikeSpecifications);
            var users = await _unitOfWork.Repository<UserLike, string>().GetAllWithSpecs(UserLikeWithSpecs);


            if (userLikeSpecifications.predicate == "source")
            {
                // use auto mapper
                var LikedUsers = users.Select(x => x.LikedUser).ToList();
                return _mapper.Map<IEnumerable<MemberDTO>>(LikedUsers);
            }
            else
            {
                var SourceUsers = users.Select(x => x.SourceUser).ToList();
                return _mapper.Map<IEnumerable<MemberDTO>>(SourceUsers);
            }
        }

        public async Task<Response<Nothing>> RemoveLike(UserLikeDTO userLikeDTO)
        {
            var userLike = await _unitOfWork.Repository<UserLike, string>().Get(usr => usr.LikedUserId == userLikeDTO.LikedUserId && usr.SourceUserId == userLikeDTO.SourceUserId);

            var IsDeleted = await _unitOfWork.Repository<UserLike, string>().Delete(userLike);
            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
            {
                var response = new Response<Nothing>()
                {
                    Data = null,
                    Message = "Like Removed Successfully",
                    StatusCode = 200,
                    Status = true
                };
                return response;
            }
            else
            {
                var response = new Response<Nothing>()
                {
                    Data = null,
                    Message = "Error in removing like",
                    StatusCode = 500,
                    Status = false
                };
                return response;
            }
        }
    }
}