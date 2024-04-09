using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Sunrise> Sunrises { get; set; }
    public DbSet<Sunset> Sunsets { get; set; }
    public DbSet<MoonData> MoonData { get; set; }

    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
    }
}