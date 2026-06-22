using Microsoft.AspNetCore.Mvc;
using Heliotracker.Dtos;
using Heliotracker.Services;

namespace Heliotracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerformanceLogController : ControllerBase
{
    private readonly IPerformanceLogService _performanceLog;

    public PerformanceLogController(IPerformanceLogService performanceLog)
    {
        _performanceLog = performanceLog;
    }
    
    //Post Endpoint for data stream
    [HttpPost]
    public async Task<IActionResult> AppendLog([FromBody] LogDto dto)
    {
        try
        {
            var response = await _performanceLog.PostLogAsync(dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
    
    //Get Endpoint for underperforming panels
    [HttpGet("underperforming")]
    public async Task<IActionResult> GetUnderPerformingPanels()
    {
        try
        {
            var uPanels = await _performanceLog.GetUnderPerformingPanelIdsAsync();
            return Ok(uPanels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
    
}