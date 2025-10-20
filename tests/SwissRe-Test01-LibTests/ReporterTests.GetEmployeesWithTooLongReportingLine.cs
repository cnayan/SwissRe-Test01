using System.Diagnostics.CodeAnalysis;
using SwissRe_Test01_Lib.Models;

namespace SwissRe_Test01_LibTests;

public sealed partial class ReporterTests
{
    [Fact]
    public void GetEmployeesWithTooLongReportingLine_CircularReference_HandlesGracefully()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 1000m * 1),  // CEO
            CreateEmployee(2, 1, 1000m * 2),     // Level 1
            CreateEmployee(3, 2, 1000m * 3),     // Level 2
            CreateEmployee(4, 3, 1000m * 4),     // Level 3
            CreateEmployee(5, 4, 1000m * 5)      // Level 4
        };

        // Create circular reference
        employees[1].ManagerId = employees[4].Id;

        var result = _reporter.GetEmployeesWithTooLongReportingLine(employees, 2);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetEmployeesWithTooLongReportingLine_EmptyList_ReturnsEmpty()
    {
        var result = _reporter.GetEmployeesWithTooLongReportingLine([], 3);
        Assert.Empty(result);
    }

    [Fact]
    public void GetEmployeesWithTooLongReportingLine_ExceedsMaxDepth_ReturnsViolations()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null),  // CEO
            CreateEmployee(2, 1),     // Level 1
            CreateEmployee(3, 2),     // Level 2
            CreateEmployee(4, 3),     // Level 3
            CreateEmployee(5, 4)      // Level 4
        };

        var result = _reporter.GetEmployeesWithTooLongReportingLine(employees, 2);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.EmployeeId == 4 && r.ExcessDepth == 1);
        Assert.Contains(result, r => r.EmployeeId == 5 && r.ExcessDepth == 2);
    }

    [Fact]
    public void GetEmployeesWithTooLongReportingLine_MultipleBranches_ReturnsCorrectViolations()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null),   // CEO
            CreateEmployee(2, 1),      // Branch 1 Level 1
            CreateEmployee(3, 2),      // Branch 1 Level 2
            CreateEmployee(4, 1),      // Branch 2 Level 1
            CreateEmployee(5, 4),      // Branch 2 Level 2
            CreateEmployee(6, 5)       // Branch 2 Level 3
        };

        var result = _reporter.GetEmployeesWithTooLongReportingLine(employees, 2);

        Assert.Single(result);
        Assert.Contains(result, r => r.EmployeeId == 6 && r.ExcessDepth == 1);
    }

    [Fact]
    public void GetEmployeesWithTooLongReportingLine_OnlyCEO_ReturnsEmpty()
    {
        var employees = new List<Employee> { CreateEmployee(1, null) };
        var result = _reporter.GetEmployeesWithTooLongReportingLine(employees, 3);
        Assert.Empty(result);
    }

    [Fact]
    public void GetEmployeesWithTooLongReportingLine_WithinMaxDepth_ReturnsEmpty()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null),  // CEO
            CreateEmployee(2, 1),     // Level 1
            CreateEmployee(3, 2)      // Level 2
        };

        var result = _reporter.GetEmployeesWithTooLongReportingLine(employees, 3);
        Assert.Empty(result);
    }
}