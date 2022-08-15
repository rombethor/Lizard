using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class LogEntryAddOptions
    {
        public LogEntryAddOptions()
        {
            var name = Assembly.GetEntryAssembly()?.GetName();
            Message = string.Empty;
            if (name != null)
            {
                Source = new SourceAddOptions()
                {
                    Name = name.Name ?? string.Empty,
                    Version = name.Version?.ToString() ?? string.Empty
                };
            }
        }

        public LogEntryAddOptions(Exception ex)
        {
            var name = Assembly.GetEntryAssembly()?.GetName();
            Message = ex.Message;
            if (name != null)
            {
                Source = new SourceAddOptions()
                {
                    Name = name.Name ?? string.Empty,
                    Version = name.Version?.ToString() ?? string.Empty
                };
            }
            Exception = new ExceptionAddOptions(ex);
        }

        [Required, MaxLength(512)]
        public string Message { get; set; }

        public DateTime Occurred { get; set; } = DateTime.UtcNow;

        [Required]
        public SourceAddOptions Source { get; set; } = new SourceAddOptions();

        public ExceptionAddOptions? Exception { get; set; }

        /// <summary>
        /// Optional HTTP request and response details
        /// </summary>
        public HttpLogAddOptions? HttpDetail { get; set; }
    }
}
