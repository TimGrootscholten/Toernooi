using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;

namespace Migrations
{
    public class TournamentDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public TournamentDbContext(DbContextOptions<TournamentDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TournamentDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(x => x.ClrType.IsSubclassOf(typeof(BaseEntity))))
            {
                modelBuilder.Entity(entityType.Name, x =>
                {
                    x.Property("Created")
                        .HasDefaultValueSql("getutcdate()");

                    x.Property("Updated")
                        .HasDefaultValueSql("getutcdate()");
                });
            }
        }
    }
}
