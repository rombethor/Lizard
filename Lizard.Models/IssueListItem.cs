using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class IssueListItem
    {
        public long LogEntryID { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime LastWritten { get; set; }
        public DateTime LastOccurred { get; set; }
        public int NumberOfOccurrences { get; set; }
        public Source Source { get; set; } = new Source();

        public ExceptionDetail? Exception { get; set; }
    }
}
