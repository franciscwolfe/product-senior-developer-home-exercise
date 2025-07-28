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
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    private readonly IPersonMapper _personMapper;
    private readonly IDepartmentService _departmentService;

    public PersonController(IPersonService personService, IPersonMapper personMapper, IDepartmentService departmentService)
    {
        _personService = personService;
        _personMapper = personMapper;
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetAll()
    {
        var persons = await _personService.GetAllAsync();
        var result = persons.Select(p => _personMapper.ToViewModel(p));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonViewModel>> GetById(int id)
    {
        var person = await _personService.GetByIdAsync(id);
        if (person == null) return NotFound();
        return Ok(_personMapper.ToViewModel(person));
    }

    [HttpPost]
    public async Task<ActionResult<PersonViewModel>> Create([FromBody] PersonViewModel viewModel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        // Validate department exists
        var department = await _departmentService.GetByIdAsync(viewModel.DepartmentId);
        if (department == null)
        {
            ModelState.AddModelError(nameof(viewModel.DepartmentId), "Invalid department selected.");
            return BadRequest(ModelState);
        }
        
        var personEntity = _personMapper.ToEntity(viewModel);
        var createdPerson = await _personService.CreateAsync(personEntity);
        var result = _personMapper.ToViewModel(createdPerson);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PersonViewModel>> Update(int id, [FromBody] PersonViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest("ID mismatch");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        // Validate department exists
        var department = await _departmentService.GetByIdAsync(viewModel.DepartmentId);
        if (department == null)
        {
            ModelState.AddModelError(nameof(viewModel.DepartmentId), "Invalid department selected.");
            return BadRequest(ModelState);
        }
        
        // Check if person exists without tracking
        var existing = await _personService.GetByIdAsync(id);
        if (existing == null) return NotFound();
        
        // Create the entity and update
        var personEntity = _personMapper.ToEntity(viewModel);
        var updatedPerson = await _personService.UpdateAsync(personEntity);
        return Ok(_personMapper.ToViewModel(updatedPerson));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _personService.GetByIdAsync(id);
        if (existing == null) return NotFound();
        await _personService.DeleteAsync(id);
        return NoContent();
    }
}