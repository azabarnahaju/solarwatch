namespace SolarWatch.Services.Repository;
using Model;
public interface IMoonRepository
{
    Task<IEnumerable<MoonData>> GetAll();
    Task<MoonData?> GetByCity(int cityId);
    Task<MoonData?> GetByCityAndDate(int cityId, DateTime date);
    Task<MoonData?> GetById(int id);
    Task Add(MoonData moonData);
    Task Update(MoonData moonData);
    Task Delete(int id);
}