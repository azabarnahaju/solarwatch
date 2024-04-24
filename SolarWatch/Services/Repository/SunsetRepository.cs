using Microsoft.EntityFrameworkCore;
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
    
    public async Task<IEnumerable<SolarMovement>> GetAll()
    {
        return await _dbContext.Sunsets.ToListAsync();
    }

    public async Task<SolarMovement?> GetByCity(int cityId)
    {
        return await _dbContext.Sunsets.FirstOrDefaultAsync(s => s.CityId == cityId);
    }
    
    public async Task<SolarMovement?> GetByCityAndDate(int cityId, DateTime date)
    {
        return await _dbContext.Sunsets.FirstOrDefaultAsync(s => s.CityId == cityId && s.Date.Date == date.Date);
    }

    public async Task<SolarMovement?> GetById(int id)
    {
        return await _dbContext.Sunsets.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task Add(SolarMovement solarMovement)
    {
        _dbContext.Sunsets.Add((Sunset)solarMovement);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(SolarMovement solarMovement)
    {
        _dbContext.Sunsets.Update((Sunset)solarMovement);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var sunsetToDelete = await GetById(id);
        _dbContext.Sunsets.Remove((Sunset)sunsetToDelete);
        await _dbContext.SaveChangesAsync();
    }
}