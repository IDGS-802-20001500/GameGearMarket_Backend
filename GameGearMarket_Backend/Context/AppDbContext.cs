using Microsoft.EntityFrameworkCore;
using GameGearMarket_Backend.Model;

namespace GameGearMarket_Backend.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<Productos> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Productos>().HasKey(p => p.folio);

            base.OnModelCreating(modelBuilder);
        }

    }
}
