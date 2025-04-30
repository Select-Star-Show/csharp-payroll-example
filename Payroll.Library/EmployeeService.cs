namespace Payroll.Library;

public class EmployeeService
{
    private readonly IRepository<Employee, Guid> _repository;

    public EmployeeService(IRepository<Employee, Guid> repository)
    {
        _repository = repository;
    }

    // Create a new employee
    public async Task<Employee> CreateAsync(Employee employee)
    {
        // Generate a new Guid if not already set
        if (employee.Id == Guid.Empty)
            employee.Id = Guid.NewGuid();

        var created = await _repository.AddAsync(employee);
        await _repository.SaveChangesAsync();
        return created;
    }

    // Get a single employee by Id
    public Task<Employee?> GetByIdAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    // Get all employees
    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    // Find employees by role (case-insensitive)
    public Task<IEnumerable<Employee>> FindByRoleAsync(string role)
    {
        return _repository.FindAsync(e => e.Role.ToLower() == role.ToLower());
    }

    // Update an existing employee
    public async Task UpdateAsync(Employee employee)
    {
        await _repository.UpdateAsync(employee);
        await _repository.SaveChangesAsync();
    }

    // Delete an employee by Id
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
    
    public Task<IEnumerable<Employee>> GetRecentByRoleAsync(string role)
    {
        return _repository.GetRecentByRoleAsync(role);
    }
}