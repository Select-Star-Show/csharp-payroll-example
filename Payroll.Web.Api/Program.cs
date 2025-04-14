using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Payroll.Data.Ef;
using Payroll.Library;
using Payroll.Web.Api;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext for CockroachDB (via PostgreSQL)
builder.Services.AddDbContext<PayrollDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CockroachDB"))
    );

// Register generic repository + EmployeeService
builder.Services.AddScoped(typeof(IRepository<,>), typeof(EmployeeEfRepository<,>));
builder.Services.AddScoped<EmployeeService>();

// Add controller support
builder.Services.AddControllers();

// Add Swagger for API documentation
var app = builder.Build();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await DbInitialize.SeedEmployeesAsync(scope.ServiceProvider);
}

app.Run();