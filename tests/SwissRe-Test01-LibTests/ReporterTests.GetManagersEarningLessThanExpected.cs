using SwissRe_Test01_Lib.Models;

namespace SwissRe_Test01_LibTests;

public sealed partial class ReporterTests
{
    [Fact]
    public void GetManagersEarningLessThanExpected_EmptyList_ReturnsEmpty()
    {
        var result = _reporter.GetManagersEarningLessThanExpected([]);
        Assert.Empty(result);
    }

    [Fact]
    public void GetManagersEarningLessThanExpected_ExactlyAtThreshold_ReturnsEmpty()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 150000),    // CEO
            CreateEmployee(2, 1, 120000),       // Manager - exactly 1.20 times
            CreateEmployee(3, 2, 100000)        // Employee
        };

        var result = _reporter.GetManagersEarningLessThanExpected(employees);
        Assert.Empty(result);
    }

    [Fact]
    public void GetManagersEarningLessThanExpected_ManagerEarningLess_ReturnsManager()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 100000),    // CEO
            CreateEmployee(2, 1, 90000),        // Manager
            CreateEmployee(3, 2, 100000),       // Employee 1
            CreateEmployee(4, 2, 100000)        // Employee 2
        };

        var result = _reporter.GetManagersEarningLessThanExpected(employees);

        Assert.Equal(2, result.Count);

        var report = result[0];
        Assert.Equal(1, report.Id);
        Assert.Equal(100000m, report.ActualSalary);
        Assert.Equal(108000m, report.ExpectedSalary); // 100000 * 1.20
        Assert.Equal(8000m, report.Difference);

        report = result[1];
        Assert.Equal(2, report.Id);
        Assert.Equal(90000, report.ActualSalary);
        Assert.Equal(120000m, report.ExpectedSalary); // 100000 * 1.20
        Assert.Equal(30000m, report.Difference);
    }

    [Fact]
    public void GetManagersEarningLessThanExpected_ManagerEarningMore_ReturnsEmpty()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 156000),    // CEO
            CreateEmployee(2, 1, 130000),       // Manager
            CreateEmployee(3, 2, 100000),       // Employee 1
            CreateEmployee(4, 2, 100000)        // Employee 2
        };

        var result = _reporter.GetManagersEarningLessThanExpected(employees);
        Assert.Empty(result);
    }

    [Fact]
    public void GetManagersEarningLessThanExpected_MultipleManagersUnderExpected_ReturnsAll()
    {
        var employees = new List<Employee>
        {
            CreateEmployee(1, null, 200000),    // CEO
            CreateEmployee(2, 1, 90000),        // Manager 1
            CreateEmployee(3, 2, 100000),       // Employee under Manager 1
            CreateEmployee(4, 1, 110000),       // Manager 2
            CreateEmployee(5, 4, 120000)        // Employee under Manager 2
        };

        var result = _reporter.GetManagersEarningLessThanExpected(employees);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Id == 2);
        Assert.Contains(result, r => r.Id == 4);
    }

    [Fact]
    public void GetManagersEarningLessThanExpected_SingleEmployee_ReturnsEmpty()
    {
        var employees = new List<Employee> { CreateEmployee(1, null, 100000) };
        var result = _reporter.GetManagersEarningLessThanExpected(employees);
        Assert.Empty(result);
    }
}
