using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Teaching
    {
        public int Id { get; set; }
        public int IdTeacher { get; set; }
        public int IdSubject { get; set; }
        public int? IdClass { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Class? IdClassNavigation { get; set; }
        public virtual Subject IdSubjectNavigation { get; set; } = null!;
        public virtual Teacher IdTeacherNavigation { get; set; } = null!;
    }
}
