using Microsoft.EntityFrameworkCore;
using TechChallenge.Core.Calendar.Entities;

namespace TechChallenge.Core.Calendar.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options)
            : base(options)
        {
        }

        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<User> Users { get; set; }
    }
}