using Microsoft.EntityFrameworkCore;

namespace LabourInc_Inquiry_Systems.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
    }
    

}
