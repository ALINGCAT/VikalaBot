using Microsoft.EntityFrameworkCore;
using VikalaBot.Database.Entities;

namespace VikalaBot.Database
{
    public class VikalaDbContext : DbContext
    {
        public DbSet<DrawInfo> DrawInfos { get; set; }
        public VikalaDbContext(DbContextOptions<VikalaDbContext> options) : base(options)
        {

        }
    }
}