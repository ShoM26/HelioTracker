namespace Heliotracker.Entities;

public class Site
{
    public int SiteId { get; set; }
    public string? SiteName { get; set; }
    public decimal SiteLatitude { get; set; }
    public decimal SiteLongitude { get; set; }
    
    public ICollection<Grid> Grids { get; set; } = new List<Grid>();
}