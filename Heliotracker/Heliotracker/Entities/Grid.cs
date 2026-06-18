namespace Heliotracker.Entities;

public class Grid
{
    public int GridId { get; set; }
    public int SiteId { get; set; }
    
    public required Site Site  { get; set; }
    
    public ICollection<Panel> Panels { get; set; } = new List<Panel>();
    
}