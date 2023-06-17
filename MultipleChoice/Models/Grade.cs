using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Grade
    {
        public Grade()
        {
            Classes = new HashSet<Class>();
            Subjects = new HashSet<Subject>();
        }

        public int Id { get; set; }
        public string? GradeName { get; set; }
        public string? Meta { get; set; }
        public byte? IsDelete { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
