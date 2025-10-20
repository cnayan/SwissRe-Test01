using System.Diagnostics.CodeAnalysis;

namespace SwissRe_Test01_Lib.Models;

[ExcludeFromCodeCoverage]
public sealed record ManagerSalaryReport
{
    public int Id { get; set; }

    /// <summary>
    /// Full name of the manager.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Actual salary of the manager.
    /// </summary>
    public decimal ActualSalary { get; set; }

    /// <summary>
    /// Expected salary of the manager.
    /// </summary>
    public decimal ExpectedSalary { get; set; }

    /// <summary>
    /// Excess or deficit amount compared to expected salary.
    /// </summary>
    public decimal Difference { get; set; }
}
