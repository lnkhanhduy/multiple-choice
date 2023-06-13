using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Chapters = new HashSet<Chapter>();
            Teachings = new HashSet<Teaching>();
        }

        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? Meta { get; set; }
        public byte? IsDelete { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
