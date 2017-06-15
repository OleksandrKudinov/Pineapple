using System;

namespace Pineapple.Database.Models
{
    public sealed class Account
    {
        public Int32 AccountId { get; set; }
        public User User { get; set; }
        public String Login { get; set; }
        public String PasswordHash { get; set; }
    }
}
