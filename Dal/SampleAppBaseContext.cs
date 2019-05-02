using LogoPaasSampleApp.Dal.Entity;
using Microsoft.EntityFrameworkCore;
using NAFCore.DAL.EF.Core;
using System.Globalization;

namespace LogoPaasSampleApp.Dal
{

    public class SampleAppBaseContext : BaseContext
    {
        public DbSet<Customer> Customer { get; set; }

        public SampleAppBaseContext(DbContextOptions options) : base(options)
        {

        }

        public SampleAppBaseContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    property.Relational().ColumnName = property.Relational().ColumnName.ToUpper(CultureInfo.GetCultureInfo("en-US"));
                }
            }
            
        }
    }
}
