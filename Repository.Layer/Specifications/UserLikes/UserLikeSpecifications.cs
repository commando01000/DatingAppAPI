using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Layer.Specifications.UserLikes
{
    public class UserLikeSpecifications
    {
        public string? SourceUserId { get; set; }
        public string? LikedUserId { get; set; }
        public string? userId { get; set; }
        public string? predicate { get; set; }
    }
}
