using Data.Layer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Layer.Contexts.Configurations
{
    public class UserMessagesConfigurations : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(x => x.Sender).WithMany(m => m.MessagesSent).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Recipient).WithMany(m => m.MessagesReceived).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
