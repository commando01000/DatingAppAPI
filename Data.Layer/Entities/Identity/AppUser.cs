using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Data.Layer.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; } // AkA known as
        public string? Bio { get; set; }
        public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateTime LastActive { get; set; } = DateTime.Now;
        public Address? Address { get; set; } // Nullable Address
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public List<Photo> Photos { get; set; } = new();
    }

    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
