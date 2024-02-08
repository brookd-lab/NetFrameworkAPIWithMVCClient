using API.Model;
using System.Collections.Generic;
using System.Data.Entity;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=DefaultConnection") { }

        public DbSet<Employee> Employee { get; set; }
    }
}
