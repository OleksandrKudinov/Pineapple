using System;
using System.ComponentModel.DataAnnotations;

namespace Pineapple.Service.Models
{
    public sealed class ChatBindingModel
    {
        [Required]
        public String ChatName { get; set; }
    }
}
