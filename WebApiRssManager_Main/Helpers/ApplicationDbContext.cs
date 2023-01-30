using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApiRssManager_Main.Helpers
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
