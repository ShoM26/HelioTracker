using Heliotracker.Dtos;
using Heliotracker.Entities;

namespace Heliotracker.Services;

public interface IPerformanceLogService
{
    Task<IEnumerable<PerformanceLog>> GetUnderPerformingPanelsAsync();
    Task<PostResponseDto> PostLogAsync(LogDto dto);
}