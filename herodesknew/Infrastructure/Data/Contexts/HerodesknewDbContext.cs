using herodesknew.Domain.Entities;
using herodesknew.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace herodesknew.Infrastructure.Data.Contexts
{
    public class HerodesknewDbContext : DbContext
    {
        public DbSet<PullRequest> PullRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Data Source=HerodesknewDb.db";
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PullRequestConfiguration());
        }
    }
}
