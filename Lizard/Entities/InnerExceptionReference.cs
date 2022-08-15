using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lizard.Entities
{
    [Table(nameof(InnerExceptionLogEntry), Schema = Schema.Name)]
    public class InnerExceptionReference
    {
        public long OuterExceptionLogEntryID { get; set; }

        [Key]
        public long InnerExceptionLogEntryID { get; set; }

        [ForeignKey(nameof(InnerExceptionLogEntryID))]
        public virtual ExceptionLogEntry? InnerExceptionLogEntry { get; set; }

        [ForeignKey(nameof(OuterExceptionLogEntryID))]
        public virtual ExceptionLogEntry? OuterExceptionLogEntry { get; set; }
    }
}
