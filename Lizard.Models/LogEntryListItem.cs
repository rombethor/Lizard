using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class LogEntryListItem
    {
        public long LogEntryID { get; set; }
        public long OccurrenceID { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Written { get; set; }
        public DateTime Occurred { get; set; }
        public Source Source { get; set; } = new Source();
        public bool IsException { get; set; }
        public bool IsHttp { get; set; }
    }
}
