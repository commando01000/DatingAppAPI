using Data.Layer.Entities.Identity;
using Repository.Layer.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Layer.Specifications.Users
{
    public class UserWithSpecifications : BaseSpecifications<AppUser>
    {
        public UserWithSpecifications(UserSpecifications spec)
       : base(user =>
           (string.IsNullOrEmpty(spec.Search) || user.DisplayName.ToLower().Contains(spec.Search.ToLower())) &&
           (string.IsNullOrEmpty(spec.Id) || user.Id == spec.Id) &&
           (string.IsNullOrEmpty(spec.Email) || user.Email == spec.Email) &&
           (string.IsNullOrEmpty(spec.Interests) || user.Interests.Contains(spec.Interests)) &&
           (string.IsNullOrEmpty(spec.Gender) || user.Gender == spec.Gender) &&
           (!spec.MinDob.HasValue || user.DateOfBirth >= spec.MinDob.Value) &&
           (!spec.MaxDob.HasValue || user.DateOfBirth <= spec.MaxDob.Value) &&
           (!spec.ExactDob.HasValue || user.DateOfBirth == spec.ExactDob.Value)
       )
        {
            AddInclude(user => user.Photos);
            AddInclude(user => user.Address);
        }
    }
}
