using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.DTOs
{
    public class UserLikeDTO
    {
        public string SourceUserId { get; set; }
        public string LikedUserId { get; set; }
    }
}
