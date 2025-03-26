using Microsoft.EntityFrameworkCore;
public class GOBTrackerDbContext : DbContext
{
    public GOBTrackerDbContext(DbContextOptions<GOBTrackerDbContext> options) : base(options) { }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Stat> Stats { get; set; }
}
