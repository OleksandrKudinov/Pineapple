using System;
using System.Collections.Generic;

namespace Pineapple.Database.Models
{
    public sealed class Chat
    {
        public Int32 ChatId { get; set; }
        public String ChatName { get; set; }
        public ICollection<User> Users{ get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
