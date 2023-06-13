using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Learning
    {
        public string IdStudent { get; set; } = null!;
        public int IdClass { get; set; }
        public int Year { get; set; }

        public virtual Class IdClassNavigation { get; set; } = null!;
        public virtual Student IdStudentNavigation { get; set; } = null!;
    }
}
