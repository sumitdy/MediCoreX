using Microsoft.EntityFrameworkCore;
using MediCoreX.Api.Models;

namespace MediCoreX.Api.Data
{
    public class MediCoreXDbContext : DbContext
    {
        public MediCoreXDbContext(DbContextOptions<MediCoreXDbContext> options)
            : base(options)
        {
        }

        // Example table
        public DbSet<Patient> Patients { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
