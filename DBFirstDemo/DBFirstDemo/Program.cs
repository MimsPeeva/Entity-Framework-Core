using Data.Models;
using DBFirstDemo.Data;
using DBFirstDemo.Data.Models;

public class Program
{
    public static void Main(string[] args)
    {
        using SoftUniContext context = new SoftUniContext();
       
        var employees = context.Employees
            .Select(e=>new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                }
            )
            .ToList();

        foreach (var employee in employees)
        {
            Console.WriteLine((object?)$"{employee.FirstName} {employee.LastName} ({employee.JobTitle} - {employee.Salary})");
        }
    }
}