using Common.Layer;
using Repository.Layer.Specifications.UserLikes;
using Services.Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.UserLikes
{
    public interface IUserLikeService
    {
        Task<UserLikeDTO> GetUserLike(UserLikeSpecifications userLikeSpecifications);

        Task<IEnumerable<MemberDTO>> GetUserLikes(UserLikeSpecifications userLikeSpecifications);

        Task<IEnumerable<string>> GetCurrentUserLikeIds(UserLikeSpecifications specs);

        Task<Response<Nothing>> AddLike(UserLikeDTO userLikeDTO);

        Task<Response<Nothing>> RemoveLike(UserLikeDTO userLikeDTO);
    }
}
