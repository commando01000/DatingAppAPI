using Data.Layer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Layer.Entities
{
    public class UserLike
    {
        public AppUser? SourceUser { get; set; }
        public string? SourceUserId { get; set; }
        public AppUser? LikedUser { get; set; }
        public string? LikedUserId { get; set; }
    }
}
