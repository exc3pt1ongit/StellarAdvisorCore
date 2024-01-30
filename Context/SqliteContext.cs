using Microsoft.EntityFrameworkCore;
using StellarAdvisorCore.Models;

namespace StellarAdvisorCore.Context
{
    public class SqliteContext : DbContext
    {
        public DbSet<MutedUser> MutedUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=Sqlite.db");
    }
}
