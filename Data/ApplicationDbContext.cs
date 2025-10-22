#nullable enable
using CMCS.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for Claims table
        public DbSet<Claim> Claims { get; set; } = null!;
    }
}
