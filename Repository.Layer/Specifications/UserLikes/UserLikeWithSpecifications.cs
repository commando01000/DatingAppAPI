using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Layer.Entities;
using Repository.Layer.Specification;

namespace Repository.Layer.Specifications.UserLikes
{
    public class UserLikeWithSpecifications : BaseSpecifications<UserLike>
    {
        public UserLikeWithSpecifications(UserLikeSpecifications specs) : base(
            usr_like =>
            (string.IsNullOrEmpty(specs.SourceUserId) || usr_like.SourceUserId == specs.SourceUserId) &&
            (string.IsNullOrEmpty(specs.LikedUserId) || usr_like.LikedUserId == specs.LikedUserId) &&
             (
                specs.predicate == "liked" ? usr_like.SourceUserId == specs.userId :
                specs.predicate == "source" ? usr_like.LikedUserId == specs.userId :
                true
            )
        )
        {
            if (specs.predicate == "liked")
            {
                AddInclude(usr_like => usr_like.LikedUser);
            }
            else
            {
                AddInclude(usr_like => usr_like.SourceUser);
            }
        }
    }
}
