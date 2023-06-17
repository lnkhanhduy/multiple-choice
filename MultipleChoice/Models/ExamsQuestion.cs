using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class ExamsQuestion
    {
        public ExamsQuestion()
        {
            Results = new HashSet<Result>();
        }

        public long Id { get; set; }
        public int? IdExam { get; set; }
        public long? IdQuestion { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Exam? IdExamNavigation { get; set; }
        public virtual Question? IdQuestionNavigation { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
