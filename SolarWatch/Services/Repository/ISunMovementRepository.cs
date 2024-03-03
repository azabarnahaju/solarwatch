using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public interface ISolarMovementRepository
{
    IEnumerable<SolarMovement> GetAll();
    SolarMovement? GetByCity(int cityId);
    SolarMovement? GetById(int id);
    void Add(SolarMovement solarMovement);
    void Update(SolarMovement solarMovement);
    void Delete(SolarMovement solarMovement);
}