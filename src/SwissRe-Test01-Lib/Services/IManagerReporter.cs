using SwissRe_Test01_Lib.Models;

namespace SwissRe_Test01_Lib;

public interface IManagerReporter
{
    /// <summary>
    /// Returns a list of employees whose reporting line (chain of managers) is longer than the allowed maximum, and by how much.
    /// </summary>
    /// <param name="employees">List of all employees.</param>
    /// <param name="maxDepth">Maximum allowed reporting line length.</param>
    /// <returns>List of employees with reporting lines too long, and the excess length.</returns>
    List<ReportingLineTooLongReport> GetEmployeesWithTooLongReportingLine(List<Employee> employees, int maxDepth);

    /// <summary>
    /// Returns a list of managers who earn less than they should, and by how much.
    /// A manager is defined as any employee who has at least one direct report.
    /// Expected salary is defined as at least 20% more than the average of paid direct report.
    /// </summary>
    List<ManagerSalaryReport> GetManagersEarningLessThanExpected(List<Employee> employees);

    /// <summary>
    /// Returns a list of managers who earn more than they should, and by how much.
    /// A manager is defined as any employee who has at least one direct report.
    /// Expected salary is defined as at max 50% more than the average of paid direct report.
    /// </summary>
    List<ManagerSalaryReport> GetManagersEarningMoreThanExpected(List<Employee> employees);
}