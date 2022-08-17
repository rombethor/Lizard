using Microsoft.EntityFrameworkCore;
using Lizard.Entities;

namespace Lizard
{
    public class LizardDbContext : DbContext
    {
        public LizardDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()).Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade))
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<LogEntry>()
                .Property(p => p.LogEntryID)
                .UseIdentityColumn(long.MinValue, 1);
        }

        public DbSet<ExceptionLogEntry> ExceptionLogEntries => Set<ExceptionLogEntry>();
        public DbSet<HttpRequestLogEntry> HttpRequestLogEntries => Set<HttpRequestLogEntry>();
        public DbSet<HttpResponseLogEntry> HttpResponseLogEntries => Set<HttpResponseLogEntry>();
        public DbSet<InnerExceptionReference> InnerExceptionReferences => Set<InnerExceptionReference>();
        public DbSet<LogEntry> LogEntries => Set<LogEntry>();
        public DbSet<Occurrence> Occurrences => Set<Occurrence>();
        public DbSet<Source> Sources => Set<Source>();
        public DbSet<StackTrace> StackTraces => Set<StackTrace>();
    }
}
