using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Level
    {
        public Level()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string? LevelName { get; set; }
        public byte? IsDelete { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
