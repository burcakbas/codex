namespace EmployeesApi.Models;

public class Employee
{
    public int Id { get; set; }
    public string SSN { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}
