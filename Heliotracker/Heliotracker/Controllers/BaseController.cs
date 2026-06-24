using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Heliotracker.Services;

namespace Heliotracker.Controllers;

public abstract class BaseController<TEntity> : ControllerBase where TEntity : class
{
    protected readonly BaseService<TEntity> _service;

    protected BaseController(BaseService<TEntity> service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
    {
        var entities = await _service.GetAllAsync();
        return Ok(entities);
    }

    [HttpGet]
    public async Task<ActionResult<TEntity>> GetById(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        return Ok(entity);
    }
}