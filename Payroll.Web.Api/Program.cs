using Microsoft.EntityFrameworkCore;
using Payroll.Data.Ado;
using Payroll.Data.Ef;
using Payroll.Library;
using Payroll.Web.Api;


// Step 0. Args
const string dbConnection = "CockroachDB";
string? dataAccessTech = args
    .SkipWhile(a => a != "--use")
    .Skip(1)
    .FirstOrDefault()?.ToLowerInvariant() ?? "adonet"; // default fallback



// Step 1. Create the Web App Builder
var builder = WebApplication.CreateBuilder(args);

// Step 2,3. Register the Data Access Layer
switch (dataAccessTech)
{
    case "efcore":
        builder.Services.AddDbContext<PayrollDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString(dbConnection)));

        builder.Services.AddScoped(typeof(IRepository<,>), typeof(EmployeeEfRepository<,>));
        break;

    case "adonet":
        builder.Services.AddScoped<IRepository<Employee, Guid>>(_ =>
            new EmployeeAdoRepository(builder.Configuration.GetConnectionString(dbConnection)));
        break;

    default:
        throw new InvalidOperationException($"Unknown data access tech: {dataAccessTech}. Use 'efcore' or 'adonet'.");
}


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