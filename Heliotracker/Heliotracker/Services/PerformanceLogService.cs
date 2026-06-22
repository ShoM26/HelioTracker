using Heliotracker.Dtos;
using Heliotracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heliotracker.Services;

public class PerformanceLogService : IPerformanceLogService
{
    private readonly AppDbContext _context;

    public PerformanceLogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<int>> GetUnderPerformingPanelIdsAsync()
    {
        //Select all most recent readings for the panels and grab the ones where concerned == 1
        var panelIds = await _context.PerformanceLogs.Where(l => l.Concerned)
            .GroupBy(l => l.PerformanceLogId)
            .Select(l => l.OrderByDescending(pl => pl.Timestamp).Select(pl => pl.PanelId).First())
            .ToListAsync();
        return panelIds;
    }

    public async Task<PostResponseDto> PostLogAsync(LogDto dto)
    {
        var logData = await _context.PerformanceLogs
            .Where(l => l.PerformanceLogId == dto.PerformanceLogId)
            .Select(l => new
            {
                Panel = l.Panel,
                Latitude = (double)l.Panel.Grid.Site.SiteLatitude,
                Longitude = (double)l.Panel.Grid.Site.SiteLongitude,
                PeakPower = (double)l.Panel.PeakCapacityKw
            }).FirstAsync();
        var log = new PerformanceLog
        {
            PerformanceLogId = dto.PerformanceLogId,
            PanelId = dto.PanelId,
            Timestamp = dto.Date,
            EnergyGeneratedKw = dto.EnergyGenerated,
            Panel = logData.Panel
        };
        //time is stored in utc in the dtos. we have to grab the grid the panel is associated and the site where the grid is at
        //The site has the lat, lon information which we can use to calculate the local solar time
        //from this calculate which panels are underperforming based on the % of full sun
        //Once the calculation is done we set the boolean concerned variable below and write it to the database
        var availableSunPercentage = SolarMathHelper.GetAvailableSunPercentage(logData.Latitude, logData.Longitude, dto.Date);
        //Only care about values where above 20 degrees

        var expectedPowerKw = logData.PeakPower * availableSunPercentage;
        
        const double timeInterval = 0.25;
        var expectedEnergy = expectedPowerKw * timeInterval;

        if (expectedEnergy > 0.2)
        {
            var relativePowerPercentage = ((double)dto.EnergyGenerated /expectedEnergy)*100;
            var underperforming = relativePowerPercentage < 0.75;
            log.Concerned = underperforming;
        }
        
        await _context.PerformanceLogs.AddAsync(log);
        await _context.SaveChangesAsync();
        return new PostResponseDto
        {
            IsSuccess = true,
            Message = "Log Added Successfully",
            Log = log
        };
    }
}