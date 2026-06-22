namespace Heliotracker.Dtos;

public class LogDto
{
    public int PerformanceLogId { get; set; }
    public int PanelId { get; set; }
    public DateTime Date { get; set; } =  DateTime.UtcNow;
    public decimal EnergyGenerated {get; set;}
}