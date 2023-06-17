using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Question
    {
        public Question()
        {
            ExamsQuestions = new HashSet<ExamsQuestion>();
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
        public int? Approver { get; set; }
        public int? IdLevel { get; set; }
        public int? IdLesson { get; set; }
        public int? Author { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Editor { get; set; }
        public DateTime? EditingDate { get; set; }
        public int? Eraser { get; set; }
        public DateTime? DeletionDate { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Answer? IdAnswerNavigation { get; set; }
        public virtual Lesson? IdLessonNavigation { get; set; }
        public virtual Level? IdLevelNavigation { get; set; }
        public virtual ICollection<ExamsQuestion> ExamsQuestions { get; set; }
    }
}
