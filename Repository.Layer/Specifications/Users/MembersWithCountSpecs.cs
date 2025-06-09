using Data.Layer.Entities.Identity;
using Repository.Layer.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Layer.Specifications.Users
{
    public class MembersWithCountSpecs : BaseSpecifications<AppUser>
    {
        public MembersWithCountSpecs(MemberSpecifications spec)
           : base(user =>
               (string.IsNullOrEmpty(spec.Id) || user.Id == spec.Id) &&
               (string.IsNullOrEmpty(spec.Search) || user.DisplayName.ToLower().Contains(spec.Search.ToLower())) &&
               (string.IsNullOrEmpty(spec.Email) || user.Email == spec.Email) &&
               (string.IsNullOrEmpty(spec.Interests) || user.Interests.Contains(spec.Interests)) &&
               (string.IsNullOrEmpty(spec.LookingFor) || user.LookingFor.Contains(spec.LookingFor)) &&
               (string.IsNullOrEmpty(spec.Gender) || user.Gender == spec.Gender) &&

               (!spec.MinDob.HasValue || user.DateOfBirth >= spec.MinDob.Value) &&
               (!spec.MaxDob.HasValue || user.DateOfBirth <= spec.MaxDob.Value) &&

               (!spec.ExactDobStart.HasValue || user.DateOfBirth >= spec.ExactDobStart.Value) &&
               (!spec.ExactDobEnd.HasValue || user.DateOfBirth <= spec.ExactDobEnd.Value)

           )
        {
        }

    }
}
