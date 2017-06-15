using System;
using Pineapple.Database.Models;

namespace Pineapple.Service.Models.View
{
    public sealed class MessageViewModel
    {
        public Int32 MessageId { get; set; }
        public Int32 ChatId { get; set; }
        public User User { get; set; }
        public String Text { get; set; }
        public DateTime SendDate { get; set; }
    }
}
