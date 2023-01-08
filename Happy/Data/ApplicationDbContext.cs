using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Happy.Models;

namespace Happy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Happy.Models.Foods> Foods { get; set; }
        public DbSet<Happy.Models.Drinks> Drinks { get; set; }
        public DbSet<Happy.Models.Restaurants> Restaurants { get; set; }
    }
}