using Microsoft.EntityFrameworkCore;
using StellarAdvisorCore.Data.Models;
using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Models.Entities.Settlements;

namespace StellarAdvisorCore.Data.Context
{
    public class SqliteContext : DbContext
    {
        public DbSet<MutedUser> MutedUsers { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<SettlementBase> Settlements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=Sqlite.db");
    }
}
