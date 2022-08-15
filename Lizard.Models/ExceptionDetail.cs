using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class ExceptionDetail
    {
        public long ExceptionLogEntryID { get; set; }

        public string Message { get; set; } = string.Empty;
        public DateTime Occurred { get; set; }
        public DateTime Written { get; set; }

        public SourceAddOptions? Source { get; set; }
        public StackTrace? StackTrace { get; set; }

        public ExceptionDetail? InnerException { get; set; }
    }
}
