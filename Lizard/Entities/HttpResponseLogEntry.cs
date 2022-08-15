using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lizard.Entities
{
    [Table(nameof(HttpResponseLogEntry), Schema = Schema.Name)]
    public class HttpResponseLogEntry
    {
        [Key]
        public long LogEntryID { get; set; }
        public int StatusCode { get; set; } = 200;

        [Required, MaxLength(512)]
        public string Content { get; set; } = string.Empty;

        [Required, MaxLength(512)]
        public string Headers { get; set; } = string.Empty;

        [ForeignKey(nameof(LogEntryID))]
        public HttpRequestLogEntry? Request { get; set; }
    }
}
