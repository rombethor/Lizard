using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class LogEntryDetail
    {
        public long LogEntryID { get; set; }
        public string Message { get; set; } = string.Empty;
        public Page<DateTime>? Occurrences { get; set; }
        public bool IsException { get; set; }
        public bool IsHttp { get; set; }
    }
}
