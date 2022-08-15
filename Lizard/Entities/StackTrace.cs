using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lizard.Entities
{
    [Table(nameof(StackTrace), Schema = Schema.Name)]
    public class StackTrace
    {
        [Key]
        public long ExceptionLogEntryID { get; set; }

        [Required, MaxLength(256)]
        public string TargetSite { get; set; } = string.Empty;

        [Required, MaxLength(5000)]
        public string Content { get; set; } = string.Empty;

        [ForeignKey(nameof(ExceptionLogEntryID))]
        public virtual ExceptionLogEntry? ExceptionLogEntry { get; set; }
    }
}
