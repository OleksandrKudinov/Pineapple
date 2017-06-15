using System;
using System.ComponentModel.DataAnnotations;

namespace Pineapple.Service.Models.Binding
{
    public sealed class AccountRegisterBindingModel
    {
        [Required]
        public String Username { get; set; }

        [Required]
        public String Password { get; set; }
    }
}
