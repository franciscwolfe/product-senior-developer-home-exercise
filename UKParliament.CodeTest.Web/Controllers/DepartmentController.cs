using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Mappers;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IDepartmentMapper _departmentMapper;

    public DepartmentController(IDepartmentService departmentService, IDepartmentMapper departmentMapper)
    {
        _departmentService = departmentService;
        _departmentMapper = departmentMapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentViewModel>>> GetAll()
    {
        var depts = await _departmentService.GetAllAsync();
        var result = depts.Select(d => _departmentMapper.ToViewModel(d));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartmentViewModel>> GetById(int id)
    {
        var dept = await _departmentService.GetByIdAsync(id);
        if (dept == null) return NotFound();
        return Ok(_departmentMapper.ToViewModel(dept));
    }
} 