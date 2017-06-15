﻿using System;

namespace Pineapple.Database.Models
{
    public sealed class Message
    {
        public Int32 MessageId { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
        public String Text { get; set; }
        public DateTime SendDate { get; set; }
    }
}
