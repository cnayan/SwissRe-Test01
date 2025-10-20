using SwissRe_Test01_Lib.Models;

namespace SwissRe_Test01_Lib;

internal sealed class ManagerReporter : IManagerReporter
{
    private Dictionary<int, List<Employee>>? _directReportsCache;

    /// <inheritdoc />
    public List<ReportingLineTooLongReport> GetEmployeesWithTooLongReportingLine(List<Employee> employees, int maxDepth)
    {
        Dictionary<int, int> _depthCache = [];
        Dictionary<int, Employee> employeeDict = employees.ToDictionary(e => e.Id);

        var ceo = employees.SingleOrDefault(e => !e.ManagerId.HasValue);
        if (ceo is null)
        {
            // No CEO found, return empty list
            return [];
        }

        List<ReportingLineTooLongReport> result = [];
        foreach (var employee in employees)
        {
            int depth = CalculateDepth(_depthCache, employee.Id, employeeDict);
            if (depth > maxDepth)
            {
                result.Add(new ReportingLineTooLongReport
                {
                    EmployeeId = employee.Id,
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    Depth = depth,
                    ExcessDepth = depth - maxDepth
                });
            }
        }

        return result;
    }

    /// <inheritdoc />
    public List<ManagerSalaryReport> GetManagersEarningLessThanExpected(List<Employee> employees)
    {
        return GetManagerSalaryReports(
            employees,
            avgSalary => avgSalary * 1.20m,
            (actual, expected) => actual < expected);
    }

    /// <inheritdoc />
    public List<ManagerSalaryReport> GetManagersEarningMoreThanExpected(List<Employee> employees)
    {
        return GetManagerSalaryReports(
            employees,
            avgSalary => avgSalary * 1.50m,
            (actual, expected) => actual > expected);
    }

    /// <summary>
    /// Calculates the depth of the reporting line for a given employee.
    /// </summary>
    /// <param name="depthCache"></param>
    /// <param name="employeeId"></param>
    /// <param name="employeeDict"></param>
    /// <returns></returns>
    private static int CalculateDepth(Dictionary<int, int> depthCache, int employeeId, Dictionary<int, Employee> employeeDict)
    {
        if (depthCache!.TryGetValue(employeeId, out int cachedDepth))
        {
            return cachedDepth;
        }

        int depth = 0;
        Employee? current = employeeDict[employeeId];
        if (current is null)
        {
            return depth;
        }

        HashSet<int> visited = [current.Id];

        while (current.ManagerId.HasValue)
        {
            if (!employeeDict.TryGetValue(current.ManagerId.Value, out Employee? manager)
                || !visited.Add(manager.Id))
            {
                break;
            }

            depth++;
            current = manager;
        }

        depthCache[employeeId] = depth;
        return depth;
    }

    /// <summary>
    /// Generates manager salary reports based on expected salary calculations and comparisons.
    /// </summary>
    /// <param name="employees">List of employees</param>
    /// <param name="expectedSalaryCalculator">Expected salary calculator</param>
    /// <param name="salaryComparer">Salary comparer - whether less than a certain amount or more than that</param>
    /// <returns>List of <see cref="ManagerSalaryReport"/></returns>
    private List<ManagerSalaryReport> GetManagerSalaryReports(
        List<Employee> employees,
        Func<decimal, decimal> expectedSalaryCalculator,
        Func<decimal, decimal, bool> salaryComparer)
    {
        if (_directReportsCache is null)
        {
            InitializeManagerCache(employees);
        }

        List<ManagerSalaryReport> managerReports = [];
        Dictionary<int, Employee> employeeDict = employees.ToDictionary(e => e.Id);

        foreach (var managerId in _directReportsCache!.Keys)
        {
            if (!employeeDict.TryGetValue(managerId, out Employee? person))
            {
                continue;
            }

            List<Employee> directReports = _directReportsCache[managerId];
            if (directReports.Count == 0)
            {
                continue;
            }

            decimal averageReportSalary = directReports.Average(e => e.Salary);
            decimal expectedSalary = expectedSalaryCalculator(averageReportSalary);

            if (salaryComparer(person.Salary, expectedSalary))
            {
                managerReports.Add(new ManagerSalaryReport
                {
                    Id = person.Id,
                    Name = $"{person.FirstName} {person.LastName}",
                    ActualSalary = Math.Round(person.Salary, 2, MidpointRounding.AwayFromZero),
                    ExpectedSalary = Math.Round(expectedSalary, 2, MidpointRounding.AwayFromZero),
                    Difference = Math.Round(Math.Abs(person.Salary - expectedSalary), 2, MidpointRounding.AwayFromZero)
                });
            }
        }

        return managerReports;
    }

    private void InitializeManagerCache(List<Employee> employees)
    {
        _directReportsCache = employees
            .Where(e => e.ManagerId.HasValue)
            .GroupBy(e => e.ManagerId!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}
