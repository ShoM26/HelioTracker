namespace Heliotracker.Entities;

public class Panel
{
    public int PanelId { get; set; }
    public int GridId { get; set; }
    public DateTime LastMaintenence { get; set; }
    public decimal PeakCapacityKw { get; set; }
    
    public required Grid Grid { get; set; }
    
    public ICollection<PerformanceLog> PerformanceLogs { get; set; } = new List<PerformanceLog>();
}