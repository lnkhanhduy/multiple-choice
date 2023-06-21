using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Chapters = new HashSet<Chapter>();
            Exams = new HashSet<Exam>();
            Teachers = new HashSet<Teacher>();
            Teachings = new HashSet<Teaching>();
        }

        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public int? IdGrade { get; set; }
        public string? Meta { get; set; }
        public int? IdLeader { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Grade? IdGradeNavigation { get; set; }
        public virtual Teacher? IdLeaderNavigation { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
