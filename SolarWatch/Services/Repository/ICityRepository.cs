using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAll();
    Task<City?> GetCity(string name);
    Task<City?> GetCity(int id);
    Task Add(City city);
    Task Update(City city);
    Task Delete(int id);
}