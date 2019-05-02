using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LogoPaasSampleApp.Dal
{
    public class PostgreSqlContext : SampleAppBaseContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// This part is used to create migrations on design time.
    /// This implementation is required due to the custom startup procedure of NAFCore.Since the EFCore migration tool looks for an
    /// implementation of a "BuildWebHost" first and cannot find that in the custom startup procedure, the tool uses the context defined
    /// in this implementation.
    /// </summary>
    public class PostgreSqlContextDesignFactory : IDesignTimeDbContextFactory<PostgreSqlContext>
    {
        public PostgreSqlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlContext>()
               .UseNpgsql("");

            return new PostgreSqlContext(optionsBuilder.Options);
        }
    }
}
