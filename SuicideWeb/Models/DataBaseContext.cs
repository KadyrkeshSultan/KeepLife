using System.Data.Entity;

namespace SuicideWeb.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            :base("name = DataBaseContext")
        {

        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Thesaurus> Thesauruss { get; set; }
        public DbSet<BlackGroup> BlackGroups { get; set; }
    }
}