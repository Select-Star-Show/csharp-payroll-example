using Microsoft.EntityFrameworkCore;
using Payroll.Data.Ef;
using Payroll.Library;

namespace Payroll.Web.Api;

public class DbInitialize
{
    public static async Task SeedEmployeesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EmployeeService>();

        var felipe = new Employee
        {
            Name = "Felipe",
            Role = "advocate"
        };
        await db.CreateAsync(felipe);
        
        var glen = new Employee
        {
            Name = "Glen",
            Role = "engineer"
        };
        await db.CreateAsync(glen);
    }
}