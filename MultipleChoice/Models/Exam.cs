using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Exam
    {
        public Exam()
        {
            Results = new HashSet<Result>();
        }

        public int Id { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? ExamTime { get; set; }
        public byte? IsDelete { get; set; }

        public virtual ICollection<Result> Results { get; set; }
    }
}
