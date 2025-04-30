using Payroll.Data.Ado;
using Payroll.Library;
using Payroll.Web.Api;

// Step 1. Create the Web App Builder
var builder = WebApplication.CreateBuilder(args);

// ========================================
// ADO.NET / EF Core What do you prefer??
// ========================================

// ========================================
// EF Core
// ========================================
// Step 2. (EF Core) Register DbContext for CockroachDB (via PostgreSQL)
// builder.Services.AddDbContext<PayrollDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("CockroachDB"))
//     );

// Step 3. (EF Core) Register the Repository and Implementation
// builder.Services.AddScoped(typeof(IRepository<,>), typeof(EmployeeEfRepository<,>));

// ========================================
// ADO.NET
// ========================================
// Step 2 and 3. (ADO.NET) Register IRepository and Implementation
builder.Services.AddScoped<IRepository<Employee, Guid>>(_ =>
    new EmployeeAdoRepository(builder.Configuration.GetConnectionString("CockroachDB")));

// Step 4. Register the EmployeeService
builder.Services.AddScoped<EmployeeService>();

// Step 5. Add controller support
builder.Services.AddControllers();

// Step 6. Create the Web App
var app = builder.Build();

// Step 7. Add the Controllers
app.MapControllers();

// (Optional) Step 8. Initialize the database
using (var scope = app.Services.CreateScope())
{
    await DbInitialize.SeedEmployeesAsync(scope.ServiceProvider);
}

// Step 9. Run the Web App
app.Run();