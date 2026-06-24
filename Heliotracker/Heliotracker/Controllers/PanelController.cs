using Heliotracker.Entities;
using Microsoft.AspNetCore.Mvc;
using Heliotracker.Services;

namespace Heliotracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PanelController : BaseController<Panel>
{
    public PanelController(BaseService<Panel> service) : base(service)
    {
        
    }
    
    
}