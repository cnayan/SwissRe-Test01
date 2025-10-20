using SwissRe_Test01_Lib.Models;

namespace SwissRe_Test01_LibTests;

public sealed partial class ReporterTests
{
    [Fact]
    public void GetManagersEarningMoreThanExpected_EmptyList_ReturnsEmpty()
    {
        var result = _reporter.GetManagersEarningMoreThanExpected([]);
        Assert.Empty(result);
    }

    [Fact]
    public void GetManagersEarningMoreThanExpected_ExactlyAtThreshold_ReturnsEmpty()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 200000),    // CEO
            CreateEmployee(2, 1, 150000),       // Manager - exactly 1.50 times
            CreateEmployee(3, 2, 100000)        // Employee
        };

        var result = _reporter.GetManagersEarningMoreThanExpected(employees);
        Assert.Empty(result);
    }

    [Fact]
    public void GetManagersEarningMoreThanExpected_ManagerEarningLess_ReturnsEmpty()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 200000),    // CEO
            CreateEmployee(2, 1, 140000),       // Manager
            CreateEmployee(3, 2, 100000),       // Employee 1
            CreateEmployee(4, 2, 100000)        // Employee 2
        };

        var result = _reporter.GetManagersEarningMoreThanExpected(employees);
        Assert.Empty(result);
    }

    [Fact]
    public void GetManagersEarningMoreThanExpected_ManagerEarningMore_ReturnsManager()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 200000),    // CEO
            CreateEmployee(2, 1, 160000),       // Manager
            CreateEmployee(3, 2, 100000),       // Employee 1
            CreateEmployee(4, 2, 100000)        // Employee 2
        };

        var result = _reporter.GetManagersEarningMoreThanExpected(employees);

        Assert.Single(result);
        var report = result[0];
        Assert.Equal(2, report.Id);
        Assert.Equal(160000, report.ActualSalary);
        Assert.Equal(150000, report.ExpectedSalary); // 100000 * 1.50
        Assert.Equal(10000, report.Difference);
    }

    [Fact]
    public void GetManagersEarningMoreThanExpected_ManagerWithUnequalReports_CalculatesCorrectly()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 200000),    // CEO
            CreateEmployee(2, 1, 200000),       // Manager
            CreateEmployee(3, 2, 80000),        // Employee 1
            CreateEmployee(4, 2, 120000)        // Employee 2
        };

        var result = _reporter.GetManagersEarningMoreThanExpected(employees);

        Assert.Single(result);
        var report = result[0];
        Assert.Equal(2, report.Id);
        Assert.Equal(200000, report.ActualSalary);
        Assert.Equal(150000, report.ExpectedSalary); // (100000) * 1.50
        Assert.Equal(50000, report.Difference);
    }

    [Fact]
    public void GetManagersEarningMoreThanExpected_MultipleManagersOverExpected_ReturnsAll()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 280000),    // CEO
            CreateEmployee(2, 1, 180000),       // Manager 1
            CreateEmployee(3, 2, 100000),       // Employee under Manager 1
            CreateEmployee(4, 1, 200000),       // Manager 2
            CreateEmployee(5, 4, 120000)        // Employee under Manager 2
        };

        var result = _reporter.GetManagersEarningMoreThanExpected(employees);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Id == 2 && r.ActualSalary == 180000);
        Assert.Contains(result, r => r.Id == 4 && r.ActualSalary == 200000);
    }

    [Fact]
    public void GetManagersEarningMoreThanExpected_SingleEmployee_ReturnsEmpty()
    {
        var employees = new List<Employee> { CreateEmployee(1, null, 100000) };
        var result = _reporter.GetManagersEarningMoreThanExpected(employees);
        Assert.Empty(result);
    }
}
