using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lizard.Controllers
{
    [Route("version")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        readonly LizardDbContext db;
        public VersionController(LizardDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult GetVersion()
        {
            var name = Assembly.GetExecutingAssembly().GetName();
            return Ok(new
            {
                Version = name.Version?.ToString(),
                Assembly = name.Name,
                Author = "Daniel James Thorne"
            });
        }

        [HttpPost("migrate")]
        public IActionResult Migrate()
        {
            db.Database.Migrate();
            return Ok();
        }
    }
}
