namespace SolarWatch.Model;

public class MoonData
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public int CityId { get; init; }
    public string CurrentPhase { get; init; }
    public string NextPhase { get; init; }
    public DateTime NextPhaseTime { get; init; }
    public DateTime MoonRise { get; init; }
    public DateTime MoonSet { get; init; }
    public double MoonFraction { get; init; }
    
}