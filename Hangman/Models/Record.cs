using System;
using System.Collections.Generic;

namespace Hangman.Models
{
    public partial class Record
    {
        public int RecordId { get; set; }
        public string UserId { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
