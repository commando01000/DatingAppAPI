using AutoMapper;
using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Layer.Interfaces;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Member
{
    public class MemberService : IMemberService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        public MemberService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork<AppDbContext> unitOfWork)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<MemberDTO>> GetMemberById(string Id)
        {
            var UserWithSpecs = new MemberWithSpecifications(Id);
            var member = await _unitOfWork.Repository<AppUser, string>().GetWithSpecs(UserWithSpecs);

            var mappedUser = _mapper.Map<MemberDTO>(member);

            return new Response<MemberDTO>()
            {
                Data = mappedUser,
                Message = "Success",
                Status = true,
                StatusCode = (int)HttpStatusCode.OK,
                RedirectURL = null
            };
        }

        public async Task<Response<PaginatedResultDTO<MemberDTO>>> GetMembersWithSpecs(MemberSpecifications specs)
        {
            if (specs.Id != null)
            {
                var UserWithSpecs = new MemberWithSpecifications(specs.Id);

                var member = await _unitOfWork.Repository<AppUser, string>().GetWithSpecs(UserWithSpecs);

                var MappedUser = _mapper.Map<MemberDTO>(member);

                var result = new PaginatedResultDTO<MemberDTO>(
                 1,
                 1,
                 1,
                 new List<MemberDTO>() { MappedUser }
             );

                return new Response<PaginatedResultDTO<MemberDTO>>()
                {
                    Data = result,
                    Message = "Success",
                    Status = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    RedirectURL = null
                };
            }

            var UsersWithSpecs = new MemberWithSpecifications(specs);

            var members = await _unitOfWork.Repository<AppUser, string>().GetAllWithSpecs(UsersWithSpecs);

            var membersCount = new MembersWithCountSpecs(specs);

            var totalCount = await _unitOfWork.Repository<AppUser, string>().GetCountAsync(membersCount); // For Pagination

            var mappedUsers = _mapper.Map<List<MemberDTO>>(members);

            var paginatedResult = new PaginatedResultDTO<MemberDTO>(
                totalCount,
                specs.PageIndex,
                specs.PageSize,
                mappedUsers
            );

            return new Response<PaginatedResultDTO<MemberDTO>>()
            {
                Data = paginatedResult,
                Message = "Success",
                Status = true,
                StatusCode = (int)HttpStatusCode.OK,
                RedirectURL = null
            };
        }

        public async Task<Response<MemberDTO>> UpdateMember(MemberDTO memberDTO)
        {
            try
            {
                // Retrieve the existing entity from the database
                var existingMember = await _unitOfWork.Repository<AppUser, string>().Get(memberDTO.Id);

                if (existingMember == null)
                {
                    return new Response<MemberDTO>
                    {
                        Data = null,
                        Message = "Member not found.",
                        Status = false,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        RedirectURL = null
                    };
                }

                // Map the updated fields
                _mapper.Map(memberDTO, existingMember);

                // Update the entity
                var result = await _unitOfWork.Repository<AppUser, string>().Update(existingMember);

                // Save changes
                var res = await _unitOfWork.CompleteAsync();

                return new Response<MemberDTO>
                {
                    Data = memberDTO,
                    Message = "Success",
                    Status = result,
                    StatusCode = (int)HttpStatusCode.OK,
                    RedirectURL = null
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new Response<MemberDTO>
                {
                    Data = null,
                    Message = "Update failed due to concurrency issue: " + ex.Message,
                    Status = false,
                    StatusCode = (int)HttpStatusCode.Conflict,
                    RedirectURL = null
                };
            }
            catch (Exception ex)
            {
                return new Response<MemberDTO>
                {
                    Data = null,
                    Message = "Error: " + ex.Message,
                    Status = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    RedirectURL = null
                };
            }
        }
    }
}
