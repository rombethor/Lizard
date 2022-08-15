using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lizard.Entities
{
    [Table(nameof(Occurrence), Schema = Schema.Name)]
    public class Occurrence
    {
        [Key]
        public long OccurrenceID { get; set; }

        public long LogEntryID { get; set; }

        public DateTime Occurred { get; set; }

        public DateTime Written { get; set; } = DateTime.UtcNow;


        [ForeignKey(nameof(LogEntryID))]
        public virtual LogEntry? LogEntry { get; set; }
    }
}
