using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Result
    {
        public string IdStudent { get; set; } = null!;
        public int IdExam { get; set; }
        public long IdQuestion { get; set; }
        public string? Answer { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Exam IdExamNavigation { get; set; } = null!;
        public virtual Question IdQuestionNavigation { get; set; } = null!;
        public virtual Student IdStudentNavigation { get; set; } = null!;
    }
}
