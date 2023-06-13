using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Question
    {
        public Question()
        {
            Results = new HashSet<Result>();
        }

        public long Id { get; set; }
        public string? QuestionContent { get; set; }
        public string? AnswerA { get; set; }
        public string? AnswerB { get; set; }
        public string? AnswerC { get; set; }
        public string? AnswerD { get; set; }
        public int? IdAnswer { get; set; }
        public string? Note { get; set; }
        public byte? IsApprove { get; set; }
        public int? IdLevel { get; set; }
        public int? IdLesson { get; set; }
        public int? Author { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Editor { get; set; }
        public DateTime? EditingDate { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Answer? IdAnswerNavigation { get; set; }
        public virtual Lesson? IdLessonNavigation { get; set; }
        public virtual Level? IdLevelNavigation { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}
