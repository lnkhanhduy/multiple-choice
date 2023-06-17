using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Result
    {
        public string IdStudent { get; set; } = null!;
        public long IdExamQuestion { get; set; }
        public string? Answer { get; set; }
        public byte? IsDelete { get; set; }

        public virtual ExamsQuestion IdExamQuestionNavigation { get; set; } = null!;
        public virtual Student IdStudentNavigation { get; set; } = null!;
    }
}
