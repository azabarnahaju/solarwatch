using Microsoft.EntityFrameworkCore;
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
    
    public async Task<IEnumerable<City>> GetAll()
    {
        return await _dbContext.Cities.ToListAsync();
    }

    public async Task<City?> GetCity(string name)
    {
        return await _dbContext.Cities.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }
    
    public async Task<City?> GetCity(int id)
    {
        return await _dbContext.Cities.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task Add(City city)
    {
        _dbContext.Cities.Add(city);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(City city)
    {
        _dbContext.Cities.Update(city);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var cityToDelete = await GetCity(id);
        _dbContext.Cities.Remove(cityToDelete);
        await _dbContext.SaveChangesAsync();
    }
}