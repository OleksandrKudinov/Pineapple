using System;
using System.ComponentModel.DataAnnotations;

namespace Pineapple.Service.Models.Binding
{
    public sealed class MessageBindingModel
    {
        [Required]
        public String Text { get; set; }
    }
}
