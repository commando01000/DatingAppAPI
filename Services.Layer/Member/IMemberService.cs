using Common.Layer;
using Repository.Layer.Specifications.Users;
using Services.Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Member
{
    public interface IMemberService
    {
        public Task<Response<PaginatedResultDTO<MemberDTO>>> GetMembersWithSpecs(MemberSpecifications userSpecifications);

        public Task<Response<MemberDTO>> GetMemberById(string Id);
    }
}
