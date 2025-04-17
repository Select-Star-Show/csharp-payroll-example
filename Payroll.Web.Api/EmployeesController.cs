namespace Payroll.Web.Api;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Payroll.Data.Ef;
using Payroll.Library;


[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeesController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        var created = await _employeeService.CreateAsync(employee);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _employeeService.DeleteAsync(id);
        return Ok();
    }
    
    [HttpGet("recent/role/{role}")]
    public async Task<IActionResult> GetRecentByRole(string role, [FromServices] PayrollDbContext dbContext)
    {
        // NOTE: Parameterized to avoid SQL injection
        const string sql = @"SELECT * FROM employees AS OF SYSTEM TIME '-30s' WHERE role = @p0";

        var results = await dbContext.Employees
            .FromSqlRaw(sql,role)
            .ToListAsync();

        return Ok(results);
    }

}