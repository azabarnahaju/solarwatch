using SolarWatch.Model;

namespace SolarWatch.Services.Repository;

public interface ICityRepository
{
    IEnumerable<City> GetAll();
    City? GetCity(string name);
    City? GetCity(int id);
    void Add(City city);
    void Update(City city);
    void Delete(City city);
}