using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lizard.Controllers
{
    [Route("issue")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        LizardDbContext db;
        public IssueController(LizardDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetLatestIssues(int limit = 50, int offset = 0)
        {
            var logEntryIds = db.Occurrences.OrderBy(o => o.Written)
                .GroupBy(o => o.LogEntryID)
                .Select(o => o.Key)
                .Skip(offset).Take(limit)
                .AsEnumerable();

            var logs = db.LogEntries.Include(le => le.Exception).Include(le => le.Source)
                .Join(logEntryIds, le => le.LogEntryID, ids => ids, (le, id) => le)
                .Take(limit)
                .Select(o => new Models.IssueListItem()
                {
                    Message = o.Message,
                    LogEntryID = o.LogEntryID,
                    LastOccurred = o.Occurrences.Max(c => c.Occurred),
                    LastWritten = o.Occurrences.Max(c => c.Written),
                    NumberOfOccurrences = o.Occurrences.Count(),
                    Source = new Models.Source()
                    {
                        Name = o.Source!.Name,
                        Version = o.Source.Version,
                        SourceID = o.SourceID
                    }
                });
            return Ok(logs);
        }

        [HttpGet("{logEntryID:int}/occurrences")]
        public IActionResult GetIssueOccurrence(long logEntryID, DateTime fromDate, DateTime toDate)
        {
            if (fromDate >= toDate)
            {
                ModelState.AddModelError("fromDate", "Must be less than toDate");
                return BadRequest(ModelState);
            }

            if (!db.LogEntries.Any(le => le.LogEntryID == logEntryID))
            {
                ModelState.AddModelError("logEntryID", "No such log entry");
                return BadRequest(ModelState);
            }

            var occurrences = db.Occurrences
                .Where(o => o.LogEntryID == logEntryID && o.Occurred >= fromDate && o.Occurred <= toDate)
                .OrderBy(o => o.Occurred)
                .Select(o => new { Occurred = o.Occurred });

            return Ok(occurrences);
        }


    }
}
