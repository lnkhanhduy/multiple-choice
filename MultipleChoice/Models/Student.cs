using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Student
    {
        public Student()
        {
            Learnings = new HashSet<Learning>();
            Results = new HashSet<Result>();
        }

        public string IdStudent { get; set; } = null!;
        public string? Password { get; set; }
        public string? StudentName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public byte? IsDelete { get; set; }

        public virtual ICollection<Learning> Learnings { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
