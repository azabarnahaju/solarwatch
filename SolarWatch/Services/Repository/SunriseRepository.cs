using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public class SunriseRepository : ISolarMovementRepository
{
    public IEnumerable<SolarMovement> GetAll()
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Sunrises.ToList();
    }

    public SolarMovement? GetByCity(int cityId)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Sunrises.FirstOrDefault(s => s.CityId == cityId);
    }

    public SolarMovement? GetByCityAndDate(int cityId, DateTime date)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Sunrises.FirstOrDefault(s => s.CityId == cityId && s.Date == date);
    }
    
    public SolarMovement? GetById(int id)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.Sunrises.FirstOrDefault(s => s.Id == id);
    }

    public void Add(SolarMovement sunMovement)
    {
        using var dbContext = new SolarWatchContext();
        dbContext.Sunrises.Add((Sunrise)sunMovement);
        dbContext.SaveChanges();
    }

    public void Update(SolarMovement sunMovement)
    {
        using var dbContext = new SolarWatchContext();
        dbContext.Sunrises.Update((Sunrise)sunMovement);
        dbContext.SaveChanges();
    }

    public void Delete(SolarMovement sunMovement)
    {
        using var dbContext = new SolarWatchContext();
        dbContext.Sunrises.Remove((Sunrise)sunMovement);
        dbContext.SaveChanges();
    }
}