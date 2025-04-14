using Microsoft.EntityFrameworkCore;
using Payroll.Data.Ef;
using Payroll.Library;

namespace Payroll.Web.Api;

public class DbInitialize
{
    public static async Task SeedEmployeesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PayrollDbContext>();

        if (!await db.Employees.AnyAsync())
        {
            db.Employees.AddRange(new List<Employee>
            {
                new Employee
                {
                    Name = "Felipe",
                    Role = "advocate"
                },
                new Employee
                {
                    Name = "Glen",
                    Role = "engineer"
                }
            });

            await db.SaveChangesAsync();
        }
    }
}