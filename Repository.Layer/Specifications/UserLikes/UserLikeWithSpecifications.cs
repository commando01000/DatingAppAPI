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
                specs.predicate == "liked" ? usr_like.LikedUserId == specs.userId :
                specs.predicate == "source" ? usr_like.SourceUserId == specs.userId :
                true
            )
        )
        {
            //// Always include the user you're going to project
            //if (specs.predicate == "liked" || specs.predicate == "source")
            //{
            //    AddInclude(usr_like => usr_like.LikedUser);
            //    AddInclude(usr_like => usr_like.LikedUser!.Photos);
            //}
            //else
            //{
            //    AddInclude(usr_like => usr_like.SourceUser);
            //    // include the main photo too
            //    AddInclude(usr_like => usr_like!.SourceUser!.Photos);
            //}

            if (specs.predicate == "liked")
            {
                //  People who liked me → I want to return SourceUser
                AddInclude(usr_like => usr_like.SourceUser);
                AddInclude(usr_like => usr_like.SourceUser!.Photos.Where(p => p.IsMain));
            }
            else if (specs.predicate == "source")
            {
                // People I liked → I want to return LikedUsers
                AddInclude(usr_like => usr_like.LikedUser);
                AddInclude(usr_like => usr_like.LikedUser!.Photos.Where(p => p.IsMain));
            }
        }
    }
}
