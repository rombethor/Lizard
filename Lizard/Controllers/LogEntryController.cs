using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lizard.Controllers
{
    [Route("log-entry")]
    [ApiController]
    public class LogEntryController : ControllerBase
    {
        readonly LizardDbContext db;
        public LogEntryController(LizardDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult GetLatest(int limit = 50, int offset = 0)
        {
            var logEntryIds = db.Occurrences.OrderBy(o => o.Written)
                .GroupBy(o => o.LogEntryID)
                .Select(o => o.Key)
                .Skip(offset).Take(limit)
                .AsEnumerable();

            var logs = db.Occurrences.Include(o => o.LogEntry).ThenInclude(le => le!.Exception)
                .OrderByDescending(o => o.Written)
                .Take(limit)
                .Select(o => new Models.LogEntryListItem()
                {
                    OccurrenceID = o.OccurrenceID,
                    LogEntryID = o.LogEntryID,
                    Message = o.LogEntry!.Message,
                    Occurred = o.Occurred,
                    Written = o.Written,
                    Source = new Models.Source()
                    {
                        Name = o.LogEntry!.Source!.Name,
                        Version = o.LogEntry.Source.Version
                    }
                });
            return Ok(logs);
        }

        [HttpGet("{logEntryID:int}")]
        public IActionResult GetLogEntry(long logEntryID)
        {
            var entry = db.LogEntries.Where(le => le.LogEntryID == logEntryID)
                .Select(le => new
                {
                    LogEntryID = le.LogEntryID,
                    Message = le.Message,
                    Occurrences = new
                    {
                        Items = le.Occurrences
                            .OrderByDescending(o => o.Occurred)
                            .Select(o => o.Occurred)
                            .Take(50),
                        Limit = 50,
                        Total = le.Occurrences.Count()
                    },
                    Assembly = le.Source!.Name,
                    Version = le.Source!.Version
                });
            if (entry == null)
            {
                return NotFound();
            }

            return Ok(entry);
        }

        [HttpPost]
        public IActionResult AddLogEntry(Models.LogEntryAddOptions options)
        {
            throw new NotImplementedException("Not ready: should add log entry");
        }

        [HttpDelete("{logEntryID:int}")]
        public IActionResult RemoveLogEntry(long logEntryID)
        {
            throw new NotImplementedException("Not ready: Should remove log entry");
        }

    }
}
