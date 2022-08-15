namespace Lizard.Models
{
    /// <summary>
    /// An occurrence of an entry in the log
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// The ID of the entry
        /// </summary>
        public long LogEntryID { get; set; }

        /// <summary>
        /// The ID of this occurrence
        /// </summary>
        public long OccurrenceID { get; set; }

        /// <summary>
        /// A message describing the purpose of this log entry
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The UTC date time of when it was written to the DB
        /// </summary>
        public DateTime Written { get; set; }

        /// <summary>
        /// The UTC date and time at which it was produced by the client
        /// </summary>
        public DateTime Occurred { get; set; }

        /// <summary>
        /// The assembly and version in which this entry originated
        /// </summary>
        public Source Source { get; set; } = new Source();

        /// <summary>
        /// Exception detail
        /// </summary>
        public ExceptionDetail? Exception { get; set; }
        
        //TODO: HTTP detail
    }
}