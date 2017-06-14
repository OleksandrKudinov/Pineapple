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
            message.Property(x => x.Text).IsRequired().HasColumnType("nvarchar");
        }
    }
}
