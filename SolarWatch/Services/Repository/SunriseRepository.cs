using Microsoft.EntityFrameworkCore;
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
    
    public async Task<IEnumerable<SolarMovement>> GetAll()
    {
        return await _dbContext.Sunrises.ToListAsync();
    }

    public async Task<SolarMovement?> GetByCity(int cityId)
    {
        return await _dbContext.Sunrises.FirstOrDefaultAsync(s => s.CityId == cityId);
    }
  
    public async Task<SolarMovement?> GetByCityAndDate(int cityId, DateTime date)
    {
        return await _dbContext.Sunrises.FirstOrDefaultAsync(s => s.CityId == cityId && s.Date.Date == date.Date);
    }
    
    public async Task<SolarMovement?> GetById(int id)
    {
        return await _dbContext.Sunrises.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task Add(SolarMovement sunMovement)
    {
        _dbContext.Sunrises.Add((Sunrise)sunMovement);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(SolarMovement sunMovement)
    {
        _dbContext.Sunrises.Update((Sunrise)sunMovement);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var sunriseToDelete = await GetById(id);
        _dbContext.Sunrises.Remove((Sunrise)sunriseToDelete);
        await _dbContext.SaveChangesAsync();
    }
}