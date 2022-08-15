using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lizard.Entities
{
    [Table(nameof(HttpRequestLogEntry), Schema = Schema.Name)]
    public class HttpRequestLogEntry
    {
        [Key]
        public long LogEntryID { get; set; } = 0;

        [Required, MaxLength(10)]
        public string Method { get; set; } = "GET";

        [Required, MaxLength(512)]
        public string Content { get; set; } = string.Empty;

        [Required, MaxLength(512)]
        public string Headers { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Uri { get; set; } = string.Empty;

        [ForeignKey(nameof(LogEntryID))]
        public virtual LogEntry? LogEntry { get; set; }

        public virtual HttpResponseLogEntry? Response { get; set; }

    }
}
