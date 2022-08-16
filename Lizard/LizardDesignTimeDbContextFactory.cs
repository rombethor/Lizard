using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lizard
{
    public class LizardDesignTimeDbContextFactory : IDesignTimeDbContextFactory<LizardDbContext>
    {
        public LizardDbContext CreateDbContext(string[] args)
        {
            if (!args.Any())
                throw new ArgumentException("Connection String?  Use -Args <connectionString>");
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(args[0]);
            return new LizardDbContext(optionsBuilder.Options);
        }
    }
}
