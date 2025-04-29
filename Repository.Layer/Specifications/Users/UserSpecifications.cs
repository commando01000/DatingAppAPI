using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Layer.Specifications.Users
{
    public class UserSpecifications
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        public DateOnly? MinDob =>
            MaxAge.HasValue ? DateOnly.FromDateTime(DateTime.Today.AddYears(-MaxAge.Value - 1).AddDays(1)) : null;

        public DateOnly? MaxDob =>
            MinAge.HasValue ? DateOnly.FromDateTime(DateTime.Today.AddYears(-MinAge.Value)) : null;

        public DateOnly? ExactDob =>
            Age.HasValue ? DateOnly.FromDateTime(DateTime.Today.AddYears(-Age.Value)) : null;

        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public string? Gender { get; set; }

        public string? Sort { get; set; }
        public int pageSize { get; set; } = 6;
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 50;

        private string? _search;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }
    }
}
