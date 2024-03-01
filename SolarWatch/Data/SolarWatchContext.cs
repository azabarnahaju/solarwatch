using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Sunrise> Sunrises { get; set; }
    public DbSet<Sunset> Sunsets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=DaBaPaWo2024;Encrypt=false;");
    }
}