using EmployeesApi.Data;
using EmployeesApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration["ConnectionStrings__Default"]
    ?? "";

builder.Services.AddDbContext<EmployeesDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();

var allowedOrigins = (builder.Configuration["ALLOWED_ORIGINS"] ?? "")
    .Split(';', ',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EmployeesDbContext>();
    db.Database.Migrate();
    if (!db.Employees.Any())
    {
        db.Employees.AddRange(new[]
        {
            new Employee { SSN = "111-22-3333", FirstName = "John", LastName = "Doe", Salary = 50000 },
            new Employee { SSN = "222-33-4444", FirstName = "Jane", LastName = "Smith", Salary = 60000 },
            new Employee { SSN = "333-44-5555", FirstName = "Bob", LastName = "Johnson", Salary = 55000 },
            new Employee { SSN = "444-55-6666", FirstName = "Alice", LastName = "Williams", Salary = 70000 },
            new Employee { SSN = "555-66-7777", FirstName = "Tom", LastName = "Brown", Salary = 45000 }
        });
        db.SaveChanges();
    }
}

app.UseCors();

app.MapGet("/healthz", () => Results.Json(new { status = "ok" }));

app.MapGet("/api/employees", async (EmployeesDbContext db, int page = 1, int pageSize = 50, string? sortBy = null, string dir = "asc", string? q = null) =>
{
    var query = db.Employees.AsQueryable();
    if (!string.IsNullOrWhiteSpace(q))
    {
        query = query.Where(e => e.FirstName.Contains(q) || e.LastName.Contains(q) || e.SSN.Contains(q));
    }
    query = (sortBy, dir.ToLower()) switch
    {
        ("LastName", "desc") => query.OrderByDescending(e => e.LastName),
        ("LastName", _) => query.OrderBy(e => e.LastName),
        ("Salary", "desc") => query.OrderByDescending(e => e.Salary),
        ("Salary", _) => query.OrderBy(e => e.Salary),
        _ => query.OrderBy(e => e.Id)
    };
    var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    return Results.Ok(items);
});

app.MapGet("/api/employees/{id}", async (EmployeesDbContext db, int id) =>
{
    var employee = await db.Employees.FindAsync(id);
    return employee is not null ? Results.Ok(employee) : Results.NotFound();
});

app.MapPost("/api/employees", async (EmployeesDbContext db, Employee employee) =>
{
    db.Employees.Add(employee);
    await db.SaveChangesAsync();
    return Results.Created($"/api/employees/{employee.Id}", employee);
});

app.MapPut("/api/employees/{id}", async (EmployeesDbContext db, int id, Employee input) =>
{
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    employee.SSN = input.SSN;
    employee.FirstName = input.FirstName;
    employee.LastName = input.LastName;
    employee.Salary = input.Salary;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/employees/{id}", async (EmployeesDbContext db, int id) =>
{
    var employee = await db.Employees.FindAsync(id);
    if (employee is null) return Results.NotFound();
    db.Employees.Remove(employee);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
