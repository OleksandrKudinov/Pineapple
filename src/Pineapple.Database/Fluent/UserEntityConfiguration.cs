using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Pineapple.Database.Fluent
{
    internal sealed class UserEntityConfiguration : EntityTypeConfiguration<User>
    {
        public UserEntityConfiguration()
        {
            var message = this;
            message.HasKey(x => x.UserId);
            message.Property(x => x.Name).IsRequired();
        }
    }
}
