
using NAFCore.DAL.EF.Entities;

namespace LogoPaasSampleApp.Dal.Entity
{    
    public class Customer : Entity<long>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
