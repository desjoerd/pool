using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DePool.Data;
using DePool.Pages.Admin.Users;

namespace DePool.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pool> Pools { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PoolUser> PoolUsers { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
    }
}
