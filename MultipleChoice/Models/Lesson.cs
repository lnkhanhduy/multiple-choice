using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string? LessonName { get; set; }
        public string? Meta { get; set; }
        public int? IdChapter { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Chapter? IdChapterNavigation { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
