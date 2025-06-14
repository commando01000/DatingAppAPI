using Data.Layer.Entities;
using Data.Layer.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Layer.Contexts.Configurations
{
    public class UserLikeConfigurations : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.HasKey(x => new { x.SourceUserId, x.LikedUserId });
            builder.HasOne(x => x.SourceUser).WithMany(l => l.LikedUsers).HasForeignKey(x => x.SourceUserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.LikedUser).WithMany(l => l.LikedByUsers).HasForeignKey(x => x.LikedUserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
