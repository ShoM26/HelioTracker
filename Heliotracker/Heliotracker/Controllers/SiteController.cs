using Heliotracker.Entities;
using Microsoft.AspNetCore.Mvc;
using Heliotracker.Services;

namespace Heliotracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SiteController : BaseController<Site>
{
    public SiteController(BaseService<Site> service) : base(service)
    {
        
    }
}