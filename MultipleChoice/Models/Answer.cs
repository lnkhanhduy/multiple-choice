using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Answer
    {
        public Answer()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string? AnswerLabel { get; set; }
        public byte? IsDelete { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
