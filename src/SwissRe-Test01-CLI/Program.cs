using Microsoft.Extensions.DependencyInjection;
using SwissRe_Test01_Lib;
using SwissRe_Test01_Lib.DI;
using SwissRe_Test01_Lib.Models;
using System.Globalization;
using System.Text.Json;



Console.Write("Enter the employee CSV file path: ");
string? filePath = Console.ReadLine();
if (string.IsNullOrWhiteSpace(filePath))
{
    filePath = "C:\\Users\\nayan\\source\\repos\\SwissRe-Test01\\data\\employee_hierarchy_1000.csv";
}

List<Employee> employees = ReadEmployees(filePath);

ServiceCollection services = new();
services.AddSwissReTest01Lib();

var serviceProvider = services.BuildServiceProvider();

IManagerReporter managerReporter = serviceProvider.GetRequiredService<IManagerReporter>();

JsonSerializerOptions options = new()
{
    WriteIndented = true
};

Console.WriteLine("GetManagersEarningLessThanExpected:");
Console.WriteLine(JsonSerializer.Serialize(managerReporter.GetManagersEarningLessThanExpected(employees), options: options));

Console.WriteLine();
Console.WriteLine("GetManagersEarningMoreThanExpected:");
Console.WriteLine(JsonSerializer.Serialize(managerReporter.GetManagersEarningMoreThanExpected(employees), options: options));

Console.WriteLine();
Console.WriteLine("GetEmployeesWithTooLongReportingLine:");
Console.WriteLine(JsonSerializer.Serialize(managerReporter.GetEmployeesWithTooLongReportingLine(employees, 4), options: options));

// ================================ METHODS ================================

/// Reads employee data from a CSV file.
static List<Employee> ReadEmployees(string filePath)
{
    var employees = new List<Employee>();
    using var reader = new StreamReader(filePath);

    /* string? header = */ reader.ReadLine(); // Skip header

    while (!reader.EndOfStream)
    {
        string? line = reader.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            continue;
        }

        string[] cols = line.Split(',');
        if (cols.Length < 3)
        {
            // Ignore bad record
            continue;
        }

        employees.Add(new Employee
        {
            Id = int.Parse(cols[0], CultureInfo.InvariantCulture),
            FirstName = cols[1],
            LastName = cols[2],
            Salary = decimal.Parse(cols[3], CultureInfo.InvariantCulture),
            ManagerId = string.IsNullOrWhiteSpace(cols[4]) ? null : int.Parse(cols[4], CultureInfo.InvariantCulture)
        });
    }

    return employees;
}
