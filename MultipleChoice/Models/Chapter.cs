using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Chapter
    {
        public Chapter()
        {
            Lessons = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public string? ChapterName { get; set; }
        public string? Meta { get; set; }
        public int? IdSubject { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Subject? IdSubjectNavigation { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
