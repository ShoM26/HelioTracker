using Heliotracker.Entities;

namespace Heliotracker.Dtos;

public class PostResponseDto
{
    public bool IsSuccess { get; set; }
    public required string Message { get; set; }
    public PerformanceLog? Log { get; set; }
}