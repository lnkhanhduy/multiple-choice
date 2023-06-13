using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Member
    {
        public Member()
        {
            Results = new HashSet<Result>();
        }

        public string IdMember { get; set; } = null!;
        public string? Password { get; set; }
        public string? MemberName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Result> Results { get; set; }
    }
}
