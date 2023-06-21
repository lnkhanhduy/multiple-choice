using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Exams = new HashSet<Exam>();
            Subjects = new HashSet<Subject>();
            Teachings = new HashSet<Teaching>();
        }

        public int Id { get; set; }
        public string? IdTeacher { get; set; }
        public string? Password { get; set; }
        public string? TeacherName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? IdSubject { get; set; }
        public byte? IsDelete { get; set; }

        public virtual Subject? IdSubjectNavigation { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
