namespace Payroll.Data.Ado;

using Npgsql;
using Payroll.Library;

public class EmployeeAdoRepository : IRepository<Employee, Guid>
{
    private readonly string? _connectionString;

    public EmployeeAdoRepository(string? connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Employee> AddAsync(Employee entity)
    {
        const string sql = @"INSERT INTO employees (id, name, role)
                                 VALUES (@id, @name, @role)";
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        entity.Id = Guid.NewGuid();

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", entity.Id);
        cmd.Parameters.AddWithValue("name", entity.Name);
        cmd.Parameters.AddWithValue("role", entity.Role);
        await cmd.ExecuteNonQueryAsync();

        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = "DELETE FROM employees WHERE id = @id";
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<Employee>> FindAsync(
        System.Linq.Expressions.Expression<Func<Employee, bool>> predicate)
    {
        // Optional: could be implemented with compiled expressions, or skip in ADO variant
        throw new NotImplementedException("Use specific filtering method instead.");
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        const string sql = "SELECT id, name, role FROM employees";
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var results = new List<Employee>();

        using var cmd = new NpgsqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(new Employee
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Role = reader.GetString(2)
            });
        }

        return results;
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        const string sql = "SELECT id, name, role FROM employees WHERE id = @id";
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Employee
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Role = reader.GetString(2)
            };
        }

        return null;
    }

    public async Task UpdateAsync(Employee entity)
    {
        const string sql = @"UPDATE employees SET name = @name, role = @role WHERE id = @id";
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", entity.Id);
        cmd.Parameters.AddWithValue("name", entity.Name);
        cmd.Parameters.AddWithValue("role", entity.Role);
        await cmd.ExecuteNonQueryAsync();
    }

    public Task<int> SaveChangesAsync()
    {
        // Not needed for ADO.NET
        return Task.FromResult(0);
    }

    public async Task<IEnumerable<Employee>> GetRecentByRoleAsync(string role)
    {
        const string sql = @"SELECT id, name, role FROM employees AS OF SYSTEM TIME '-30s' WHERE role = @role";
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var list = new List<Employee>();
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("role", role);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new Employee
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Role = reader.GetString(2)
            });
        }

        return list;
    }
}