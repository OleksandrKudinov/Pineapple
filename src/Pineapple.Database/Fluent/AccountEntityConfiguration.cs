using System.Data.Entity.ModelConfiguration;
using Pineapple.Database.Models;

namespace Pineapple.Database.Fluent
{
    internal sealed class AccountEntityConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountEntityConfiguration()
        {
            var account = this;
            account.HasKey(x => x.AccountId);
            account.Property(x => x.Login).IsRequired().HasColumnType("varchar");
            account.Property(x => x.PasswordHash).IsRequired().HasColumnType("varchar");
        }
    }
}

