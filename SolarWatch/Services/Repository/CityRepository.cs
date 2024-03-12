using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public class CityRepository : ICityRepository
{
    private readonly SolarWatchContext _dbContext;

    public CityRepository(SolarWatchContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<City> GetAll()
    {
        return _dbContext.Cities.ToList();
    }

    public City? GetCity(string name)
    {
        return _dbContext.Cities.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
    }
    
    public City? GetCity(int id)
    {
        return _dbContext.Cities.FirstOrDefault(c => c.Id == id);
    }

    public void Add(City city)
    {
        _dbContext.Cities.Add(city);
        _dbContext.SaveChanges();
    }

    public void Update(City city)
    {
        _dbContext.Cities.Update(city);
        _dbContext.SaveChanges();
    }

    public void Delete(City city)
    {
        _dbContext.Cities.Remove(city);
        _dbContext.SaveChanges();
    }
}