using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class ExamDuration
    {
        public ExamDuration()
        {
            Exams = new HashSet<Exam>();
        }

        public int Id { get; set; }
        public string? DurationName { get; set; }
        public int? DurationTime { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
    }
}
