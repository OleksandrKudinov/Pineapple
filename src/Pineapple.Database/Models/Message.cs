using System;

namespace Pineapple.Database.Models
{
    public sealed class Message
    {
        public Int32 MessageId { get; set; }
        public Int32 UserFromId { get; set; }
        public Int32 UserToId { get; set; }
        public String Text { get; set; }
        public DateTime SendDate { get; set; }
    }
}
