using Heliotracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heliotracker;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Site> Sites { get; set; }
    public DbSet<Grid> Grids { get; set; }
    public DbSet<Panel> Panels { get; set; }
    public DbSet<PerformanceLog> PerformanceLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Site>()
            .HasMany(site => site.Grids)
            .WithOne(grid => grid.Site)
            .HasForeignKey(site=> site.SiteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Grid>()
            .HasMany(grid => grid.Panels)
            .WithOne(panel => panel.Grid)
            .HasForeignKey(panel=> panel.GridId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<PerformanceLog>()
            .HasOne(log => log.Panel)
            .WithMany(panel => panel.PerformanceLogs)
            .HasForeignKey(log => log.PanelId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
    }
}