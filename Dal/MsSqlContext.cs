using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Globalization;

namespace LogoPaasSampleApp.Dal
{
    public class MsSqlContext : SampleAppBaseContext
    {
        public MsSqlContext(DbContextOptions<MsSqlContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.Relational().ColumnName == "LastUpdateDate" || property.Relational().ColumnName == "InsertDate" )
                    {
                        property.Relational().ColumnType = "datetime";
                        if (property.Relational().ColumnName == "InsertDate")
                        {
                            property.Relational().DefaultValue = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss", new CultureInfo("tr-TR"));
                        }
                    }
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// This part is used to create migrations on design time.
        /// This implementation is required due to the custom startup procedure of NAFCore.Since the EFCore migration tool looks for an
        /// implementation of a "BuildWebHost" first and cannot find that in the custom startup procedure, the tool uses the context defined
        /// in this implementation.
        /// </summary>
        public class MsSqlContextDesignFactory : IDesignTimeDbContextFactory<MsSqlContext>
        {
            MsSqlContext IDesignTimeDbContextFactory<MsSqlContext>.CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MsSqlContext>()
                .UseSqlServer("Data Source=localhost;User Id=sa;Password=sapass;Initial Catalog=SAMPLEAPPDB;");

                return new MsSqlContext(optionsBuilder.Options);
            }
        }
    }
}
