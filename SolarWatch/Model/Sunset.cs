using System.Data;

namespace SolarWatch.Model;

public class Sunset
{
    public int Id { get; init; }
    public string Time { get; init; }
    public int CityId { get; init; }
}