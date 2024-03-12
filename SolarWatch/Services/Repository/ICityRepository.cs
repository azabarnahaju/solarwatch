using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAll();
    Task<City?> GetCity(string name);
    Task<City?> GetCity(int id);
    void Add(City city);
    void Update(City city);
    void Delete(City city);
}