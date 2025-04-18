using Data.Layer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public AddressDTO? Address { get; set; } // Nullable Address

    }
}
