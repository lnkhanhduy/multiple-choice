using System;
using System.Collections.Generic;

namespace MultipleChoice.Models
{
    public partial class Setting
    {
        public string Keyword { get; set; } = null!;
        public string? Value { get; set; }
        public string? Description { get; set; }
    }
}
