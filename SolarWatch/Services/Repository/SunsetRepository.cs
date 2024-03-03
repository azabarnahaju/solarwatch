using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public class SunsetRepository : ISolarMovementRepository
{
    public IEnumerable<SolarMovement> GetAll()
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Sunsets.ToList();
    }

    public SolarMovement? GetByCity(int cityId)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Sunsets.FirstOrDefault(s => s.CityId == cityId);
    }
    
    public SolarMovement? GetByCityAndDate(int cityId, DateTime date)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Sunsets.FirstOrDefault(s => s.CityId == cityId && s.Date == date);
    }

    public SolarMovement? GetById(int id)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Sunsets.FirstOrDefault(s => s.Id == id);
    }

    public void Add(SolarMovement solarMovement)
    {
        using var dbContext = new SolarWatchContext();
        dbContext.Sunsets.Add((Sunset)solarMovement);
        dbContext.SaveChanges();
    }

    public void Update(SolarMovement solarMovement)
    {
        using var dbContext = new SolarWatchContext();
        dbContext.Sunsets.Update((Sunset)solarMovement);
        dbContext.SaveChanges();
    }

    public void Delete(SolarMovement solarMovement)
    {
        using var dbContext = new SolarWatchContext();
        dbContext.Sunsets.Remove((Sunset)solarMovement);
        dbContext.SaveChanges();
    }
}