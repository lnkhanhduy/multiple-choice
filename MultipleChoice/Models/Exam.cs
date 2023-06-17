using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamsQuestions = new HashSet<ExamsQuestion>();
        }

        public int Id { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? IdDuration { get; set; }
        public int? IdSubject { get; set; }
        public int? Author { get; set; }
        public byte? IsApprove { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Teacher? AuthorNavigation { get; set; }
        public virtual ExamDuration? IdDurationNavigation { get; set; }
        public virtual Subject? IdSubjectNavigation { get; set; }
        public virtual ICollection<ExamsQuestion> ExamsQuestions { get; set; }
    }
}
