using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;

namespace SolarWatch.Services.Repository;

using Model;

public class MoonRepository : IMoonRepository
{
    private readonly SolarWatchContext _db;

    public MoonRepository(SolarWatchContext db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<MoonData>> GetAll()
    {
        return await _db.MoonData.ToListAsync();
    }

    public async Task<MoonData?> GetByCity(int cityId)
    {
        return await _db.MoonData.FirstOrDefaultAsync(m => m.CityId == cityId);
    }

    public async Task<MoonData?> GetByCityAndDate(int cityId, DateTime date)
    {
        return await _db.MoonData.FirstOrDefaultAsync(m => m.CityId == cityId && m.Date.Date == date.Date);
    }

    public async Task<MoonData?> GetById(int id)
    {
        return await _db.MoonData.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task Add(MoonData moonData)
    {
        _db.MoonData.Add(moonData);
        await _db.SaveChangesAsync();
    }

    public async Task Update(MoonData moonData)
    {
        _db.MoonData.Update(moonData);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var moonDataToDelete = await GetById(id);
        if (moonDataToDelete is null) return;
        _db.MoonData.Remove(moonDataToDelete);
        await _db.SaveChangesAsync();
    }
}