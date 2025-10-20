using System.Diagnostics.CodeAnalysis;

namespace SwissRe_Test01_Lib.Models;

[ExcludeFromCodeCoverage]
public sealed record Employee
{
    /// <summary>
    /// Unique identifier for the employee.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// First name of the employee.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the employee.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Salary of the employee.
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Identifier of the employee's manager. Null if the employee has no manager (e.g., CEO).
    /// </summary>
    public int? ManagerId { get; set; }
}
