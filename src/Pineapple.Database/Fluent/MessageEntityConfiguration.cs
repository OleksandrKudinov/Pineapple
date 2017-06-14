using System.Data.Entity.ModelConfiguration;
using Pineapple.Database.Models;

namespace Pineapple.Database.Fluent
{
    internal sealed class MessageEntityConfiguration  : EntityTypeConfiguration<Message>
    {
        public MessageEntityConfiguration()
        {
            var message = this;
            message.HasKey(x => x.MessageId);
            message.Property(x => x.SendDate).IsRequired().HasColumnType("datetime2");
            message.Property(x => x.UserFromId).IsRequired();
            message.Property(x => x.UserToId).IsRequired();
            message.Property(x => x.Text).IsRequired().HasColumnType("nvarchar");
        }
    }
}
