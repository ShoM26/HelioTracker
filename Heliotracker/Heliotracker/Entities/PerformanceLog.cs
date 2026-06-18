namespace Heliotracker.Entities;

public class PerformanceLog
{
    public int PerformanceLogId { get; set; }
    public int PanelId { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal EnergyGeneratedKw { get; set; }
    public bool Concerned { get; set; }

    public required Panel Panel { get; set; }
}