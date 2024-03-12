using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public class SunriseRepository : ISunriseRepository
{
    private readonly SolarWatchContext _dbContext;
    
    public SunriseRepository(SolarWatchContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<SolarMovement> GetAll()
    {
        return _dbContext.Sunrises.ToList();
    }

    public SolarMovement? GetByCity(int cityId)
    {
        return _dbContext.Sunrises.FirstOrDefault(s => s.CityId == cityId);
    }

    public SolarMovement? GetByCityAndDate(int cityId, DateTime date)
    {
        return _dbContext.Sunrises.FirstOrDefault(s => s.CityId == cityId && s.Date == date);
    }
    
    public SolarMovement? GetById(int id)
    {
        return _dbContext.Sunrises.FirstOrDefault(s => s.Id == id);
    }

    public void Add(SolarMovement sunMovement)
    {
        _dbContext.Sunrises.Add((Sunrise)sunMovement);
        _dbContext.SaveChanges();
    }

    public void Update(SolarMovement sunMovement)
    {
        _dbContext.Sunrises.Update((Sunrise)sunMovement);
        _dbContext.SaveChanges();
    }

    public void Delete(SolarMovement sunMovement)
    {
        _dbContext.Sunrises.Remove((Sunrise)sunMovement);
        _dbContext.SaveChanges();
    }
}