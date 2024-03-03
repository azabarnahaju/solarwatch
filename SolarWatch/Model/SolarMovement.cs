namespace SolarWatch.Model;

public abstract class SolarMovement
{
    public int Id { get; init; }
    public string Time { get; init; }
    public int CityId { get; init; }
}