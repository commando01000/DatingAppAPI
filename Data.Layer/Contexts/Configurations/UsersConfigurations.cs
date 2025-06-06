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
    public class UsersConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // add the address to the user
            builder.OwnsOne(x => x.Address, a => a.WithOwner());
            // if a user is deleted, delete it's photos
            builder.HasMany(x => x.Photos)
                   .WithOne(p => p.AppUser)
                   .HasForeignKey(p => p.AppUserId).IsRequired(true)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
