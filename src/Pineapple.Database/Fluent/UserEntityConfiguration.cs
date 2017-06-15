using System.Data.Entity.ModelConfiguration;
using Pineapple.Database.Models;

namespace Pineapple.Database.Fluent
{
    internal sealed class UserEntityConfiguration : EntityTypeConfiguration<User>
    {
        public UserEntityConfiguration()
        {
            var user = this;
            user.HasKey(x => x.UserId);
            user.Property(x => x.UserName).IsRequired();
            user.HasMany(x => x.Chats).WithMany(c => c.Users);
        }
    }
}
