using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.DTOs.Account
{
    public class LoginDTO
    {
        public string Email { get; set; }
        [DataType(DataType.Password), Required, StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
