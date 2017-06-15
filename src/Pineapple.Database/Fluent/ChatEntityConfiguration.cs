using System.Data.Entity.ModelConfiguration;
using Pineapple.Database.Models;

namespace Pineapple.Database.Fluent
{
    internal sealed class ChatEntityConfiguration : EntityTypeConfiguration<Chat>
    {
        public ChatEntityConfiguration()
        {
            var chat = this;
            chat.HasKey(x => x.ChatId);
            chat.Property(x => x.ChatName).IsRequired().HasColumnType("nvarchar");
        }
    }
}
