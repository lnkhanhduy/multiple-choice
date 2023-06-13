using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Class
    {
        public Class()
        {
            Learnings = new HashSet<Learning>();
            Teachings = new HashSet<Teaching>();
        }

        public int Id { get; set; }
        public string? ClassName { get; set; }
        public string? Meta { get; set; }
        public int? IdGrade { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Grade? IdGradeNavigation { get; set; }
        public virtual ICollection<Learning> Learnings { get; set; }
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
