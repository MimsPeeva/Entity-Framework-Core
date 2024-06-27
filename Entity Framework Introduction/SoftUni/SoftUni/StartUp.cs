using System.Linq;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new SoftUniContext();
            // Console.WriteLine(GetEmployeesFullInformation(context));
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            // Console.WriteLine(AddNewAddressToEmployee(context));
            //Console.WriteLine(GetEmployeesInPeriod(context));
            //Console.WriteLine(GetAddressesByTown(context));
            //Console.WriteLine(GetEmployee147(context));
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            //Console.WriteLine(GetLatestProjects(context));
            //Console.WriteLine(IncreaseSalaries(context));
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
            //Console.WriteLine(DeleteProjectById(context));
            //Console.WriteLine(RemoveTown(context));

        }

        //3
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            //option 1
            //return string.Join(Environment.NewLine, context.Employees
            //    .Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}")
            //    .ToArray());

            //option2
           var employees = context.Employees
                .Select(e=> new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();

           StringBuilder sb = new StringBuilder();
           foreach (var e in employees)
           {
               sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
           }
           return sb.ToString().TrimEnd();
        }

        //4
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e=>e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e=>e.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //5
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department,
                    e.Salary
                })
                .Where(e=>e.Department.Name== "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e=>e.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //6
         public static string AddNewAddressToEmployee(SoftUniContext context)
         {
             Address newAddress = new Address()
             {
                 TownId = 4,
                 AddressText = "Vitoshka 15"
             };

             var nakov = context.Employees
                 .FirstOrDefault(e => e.LastName == "Nakov");

             if (nakov != null)
             {
                 nakov.Address = newAddress;
                 context.SaveChanges();

             }
                 var employees = context.Employees
                     .OrderByDescending(e => e.AddressId)
                     .Take(10)
                     .Select(e=>e.Address.AddressText)
                     .ToList();

                 return string.Join(Environment.NewLine, employees);
         }

        //7
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Take(10)
                .Select(e => new
                {
                 EmployeeName = $"{e.FirstName} {e.LastName}",
               ManagerName = $"{e.Manager.FirstName} {e.Manager.LastName}",
               Projects = e.EmployeesProjects
                   .Where(ep=> ep.Project.StartDate.Year >=  2001 &&
                             ep.Project.StartDate.Year <= 2003)
                   .Select(ep => new
                   {
                       ProjectName = ep.Project.Name,
                       ep.Project.StartDate,
                    EndDate = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") :
                           "not finished"
                   })
                });
                

            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.EmployeeName} - Manager: {e.ManagerName}");
                if (e.Projects.Any())
                {
                    foreach (var p in e.Projects)
                    {

                        sb.AppendLine($"--{p.ProjectName} - {p.StartDate:M/d/yyyy h:mm:ss tt} - {p.EndDate}");
                    }
                }
            }
            return sb.ToString().TrimEnd();
        }

        //8
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                   a.AddressText,
                  TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                }
            )
              
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
            }
            return sb.ToString().TrimEnd();
        }

        //9
        public static string GetEmployee147(SoftUniContext context)
        {
            Employee emp = context.Employees
                .Include(e => e.EmployeesProjects)
                .ThenInclude(ep => ep.Project)
                .FirstOrDefault(e => e.EmployeeId == 147);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
            foreach (var e in emp.EmployeesProjects.OrderBy(ep=>ep.Project.Name))
            {
                sb.AppendLine(e.Project.Name);
            }

            return sb.ToString().TrimEnd();
        }

        //10
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(e => e.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerName = $"{d.Manager.FirstName} {d.Manager.LastName}",
                    Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        }).ToList() 
                })
                .ToList();

          

            StringBuilder sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.ManagerName}");

                foreach (var e in d.Employees)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //11
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p=>p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine($"{p.StartDate:M/d/yyyy h:mm:ss tt}");
            }

            return sb.ToString().TrimEnd();
        }

        //12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] departments = { "Engineering", "Tool Design", "Marketing", "Information Services" };

            foreach (var e in context.Employees
                         .Where(e => departments.Contains(e.Department.Name)))
            {
                e.Salary *= 1.12m;
            }
            context.SaveChanges();
            var employees = context.Employees
                    .Where(e => departments.Contains(e.Department.Name))
                .OrderBy(e=>e.FirstName)
                .ThenBy(e=>e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
        //13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e=>e.FirstName.ToLower().StartsWith("sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //14
        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectsToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);
            context.RemoveRange(projectsToDelete);

            context.Projects.Remove(context.Projects.Find(2));

            context.SaveChanges();

            var projects = context.Projects
                .Take(10)
                .Select(p => new
                {
                    p.Name
                })
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
               
            }

            return sb.ToString().TrimEnd();
        }

        //15
        public static string RemoveTown(SoftUniContext context)
        {

            var employeeAddresses = context.Employees
                .Where(e => e.Address.Town.Name == "Seattle");

            foreach (var employee in employeeAddresses)
            {
                employee.AddressId = null;
            }

            var townAddresses = context.Addresses
                .Where(a => a.Town.Name == "Seattle");
            int totalAddresses = townAddresses.Count();

            context.RemoveRange(townAddresses);

            context.Remove(context.Towns.FirstOrDefault(t => t.Name == "Seattle")!);

            context.SaveChanges();

            return $"{totalAddresses} addresses in Seattle were deleted";
        }
    }
}
    
