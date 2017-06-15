using System;
using System.ComponentModel.DataAnnotations;

namespace Pineapple.Service.Models.Binding
{
    public sealed class MessageBindingModel
    {
        [Required]
        public Int32 UserId { get; set; }
        [Required]
        public Int32 ChatId { get; set; }
        [Required]
        public String Text { get; set; }
    }
}
