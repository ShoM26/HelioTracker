using Heliotracker.Dtos;
using Heliotracker.Entities;

namespace Heliotracker.Services;

public interface IPerformanceLogService
{
    Task<IEnumerable<int>> GetUnderPerformingPanelIdsAsync();
    Task<PostResponseDto> PostLogAsync(LogDto dto);
}