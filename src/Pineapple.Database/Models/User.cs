using System;
using System.Collections.Generic;

namespace Pineapple.Database.Models
{
    public sealed class User
    {
        public Int32 UserId { get; set; }
        public String UserName { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Chat> Chats { get; set; }
    }
}
