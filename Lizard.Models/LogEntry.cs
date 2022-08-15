namespace Lizard.Models
{
    public class LogEntry
    {
        public long LogEntryID { get; set; }
        public long OccurrenceID { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Written { get; set; }
        public DateTime Occurred { get; set; }
        public Source Source { get; set; } = new Source();

        public ExceptionDetail? Exception { get; set; }
        
        //TODO: HTTP detail
    }
}