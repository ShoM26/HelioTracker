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

    public async Task<IEnumerable<PerformanceLog>> GetUnderPerformingPanelsAsync()
    {
        var panels = await _context.Panels
            .Include(a => a.PerformanceLogs)
            .ToListAsync();
        //get weather data for when sunrise and sunset are. gather the logs between those times
        //get date of the current reading and compare to what the current % of full sun is.
        //from this calculate which panels are underperforming and include them in the array and return that
        throw new NotImplementedException();
    }

    public async Task<PostResponseDto> PostLogAsync(LogDto dto)
    {
        throw new NotImplementedException();
    }
}