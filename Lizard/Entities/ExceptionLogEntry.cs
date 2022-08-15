using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lizard.Entities
{
    [Table(nameof(ExceptionLogEntry), Schema = Schema.Name)]
    public class ExceptionLogEntry
    {
        [Key]
        public long LogEntryID { get; set; }

        /// <summary>
        /// Base log entry
        /// </summary>
        [ForeignKey(nameof(LogEntryID))]
        public virtual LogEntry? LogEntry { get; set; }

        /// <summary>
        /// Further detail regarding the source code
        /// </summary>
        public virtual StackTrace? StackTrace { get; set; }

        [InverseProperty(nameof(InnerExceptionReference.OuterExceptionLogEntry))]
        public virtual InnerExceptionReference? InnerExceptionLogEntry { get; set; }

        [InverseProperty(nameof(InnerExceptionReference.InnerExceptionLogEntry))]
        public virtual InnerExceptionReference? OuterExceptionLogEntry { get; set; }
    }
}
