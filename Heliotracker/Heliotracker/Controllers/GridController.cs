using Heliotracker.Entities;
using Microsoft.AspNetCore.Mvc;
using Heliotracker.Services;

namespace Heliotracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GridController : BaseController<Grid>
{
    public GridController(BaseService<Grid> service) : base(service)
    {
        
    }
}