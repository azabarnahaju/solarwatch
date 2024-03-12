using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public interface ISolarMovementRepository
{
    Task<IEnumerable<SolarMovement>> GetAll();
    Task<SolarMovement?> GetByCity(int cityId);
    Task<SolarMovement?> GetByCityAndDate(int cityId, DateTime date);
    Task<SolarMovement?> GetById(int id);
    Task Add(SolarMovement solarMovement);
    Task Update(SolarMovement solarMovement);
    Task Delete(int id);
}