using DJT.EntityFrameworkCore;
using Lizard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lizard.Controllers
{
    [Route("source")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        readonly LizardDbContext db;
        public SourceController(LizardDbContext _db) {
            db = _db;
        }

        [HttpGet]
        public IActionResult GetSources(string? searchTerm = null, int limit = 500, int offset = 0)
        {
            var sources = db.Sources
                .WhereIf(!string.IsNullOrWhiteSpace(searchTerm), s => s.Name.Contains(searchTerm!))
                .Total(out int total)
                .Skip(offset).Take(limit)
                .Select(s => new Source
                {
                    SourceID = s.SourceID,
                    Name = s.Name,
                    Version = s.Version
                }).AsEnumerable();
            return Ok(sources.ToPage(total, limit, offset));
        }

        [HttpGet("{sourceID:int}")]
        public IActionResult GetSource(long sourceID)
        {
            var source = db.Sources.Where(s => s.SourceID == sourceID)
                .Select(s => new Source()
                {
                    SourceID = sourceID,
                    Name = s.Name,
                    Version = s.Version
                }).FirstOrDefault();
            if (source == null)
                return NotFound();
            return Ok(source);
        }

        [HttpGet("{sourceID:int}/log-entry")]
        public IActionResult GetLogEntriesForSource(long sourceID, int limit = 50, int offset = 0, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var entries = db.Occurrences.Where(o => o.LogEntry!.SourceID == sourceID)
                .OrderByDescending(o => o.Written)
                .WhereIf(fromDate.HasValue, o => o.Written >= fromDate!.Value)
                .WhereIf(toDate.HasValue, o => o.Written <= toDate!.Value)
                .Total(out int total)
                .Skip(offset)
                .Select(o => new LogEntry()
                {
                    LogEntryID = o.LogEntryID,
                    OccurrenceID = o.OccurrenceID,
                    Message = o.LogEntry!.Message,
                    Occurred = o.Occurred,
                    Written = o.Written
                }).AsEnumerable();
            return Ok(entries.ToPage(total, limit, offset));
        }

        [HttpGet("{sourceID:int}/daily-report")]
        public IActionResult GetDailyReportForSource(int sourceID)
        {
            //Show list of
            // { datetime, log-entry-count, exception-count }
            throw new NotImplementedException();
        }
    }
}
