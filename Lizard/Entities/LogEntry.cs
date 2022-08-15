using DJT.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lizard.Entities
{
    [Table(nameof(LogEntry), Schema = Schema.Name)]
    public class LogEntry : ISelfDefine
    {
        [Key]
        public long LogEntryID { get; set; }

        [Required, MaxLength(32)]
        public byte[] SHA256 { get; set; } = Array.Empty<byte>();

        [Required, MaxLength(512)]
        public string Message { get; set; } = string.Empty;


        /// <summary>
        /// Reference to the name and version of the originator
        /// </summary>
        public long SourceID { get; set; }

        /// <summary>
        /// The originator of this exception
        /// </summary>
        [ForeignKey(nameof(SourceID))]
        public virtual Source? Source { get; set; }

        public virtual ExceptionLogEntry? Exception { get; set; }
        public virtual HttpRequestLogEntry? HttpRequest { get; set; }
        public virtual IEnumerable<Occurrence> Occurrences { get; set; } = Array.Empty<Occurrence>();

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().HasIndex(nameof(SHA256));
        }
    }
}
