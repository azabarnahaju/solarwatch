using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public class SunsetRepository : ISunsetRepository
{
    private readonly SolarWatchContext _dbContext;

    public SunsetRepository(SolarWatchContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<SolarMovement> GetAll()
    {
        return _dbContext.Sunsets.ToList();
    }

    public SolarMovement? GetByCity(int cityId)
    {
        return _dbContext.Sunsets.FirstOrDefault(s => s.CityId == cityId);
    }
    
    public SolarMovement? GetByCityAndDate(int cityId, DateTime date)
    {
        return _dbContext.Sunsets.FirstOrDefault(s => s.CityId == cityId && s.Date == date);
    }

    public SolarMovement? GetById(int id)
    {
        return _dbContext.Sunsets.FirstOrDefault(s => s.Id == id);
    }

    public void Add(SolarMovement solarMovement)
    {
        _dbContext.Sunsets.Add((Sunset)solarMovement);
        _dbContext.SaveChanges();
    }

    public void Update(SolarMovement solarMovement)
    {
        _dbContext.Sunsets.Update((Sunset)solarMovement);
        _dbContext.SaveChanges();
    }

    public void Delete(SolarMovement solarMovement)
    {
        _dbContext.Sunsets.Remove((Sunset)solarMovement);
        _dbContext.SaveChanges();
    }
}