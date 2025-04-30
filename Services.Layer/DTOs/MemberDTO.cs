using Data.Layer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.DTOs
{
    public class MemberDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string? Gender { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public int Age { get; set; }
        public string? PhotoUrl { get; set; } // URL of the main photo
        public AddressDTO? Address { get; set; }
        public List<PhotoDTO> Photos { get; set; } = new();
    }
}
