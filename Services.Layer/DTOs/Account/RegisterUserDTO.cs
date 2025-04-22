using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.DTOs.Account
{
    public class RegisterDTO
    {
        public string? Id { get; set; }
        public string Username { get; set; }
        // error message
        [EmailAddress, Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [DataType(DataType.Password), StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string RePassword { get; set; }
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public AddressDTO? Address { get; set; } // Nullable Address
    }
}
