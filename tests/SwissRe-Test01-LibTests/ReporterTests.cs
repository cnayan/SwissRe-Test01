using SwissRe_Test01_Lib;
using SwissRe_Test01_Lib.Models;
using System.Diagnostics.CodeAnalysis;

namespace SwissRe_Test01_LibTests;

[ExcludeFromCodeCoverage]
public sealed partial class ReporterTests
{
    private readonly ManagerReporter _reporter;

    public ReporterTests()
    {
        _reporter = new ManagerReporter();
    }

    private static Employee CreateEmployee(int id, int? managerId, decimal salary = 100000) =>
        new()
        {
            Id = id,
            FirstName = $"FirstName{id}",
            LastName = $"LastName{id}",
            Salary = salary,
            ManagerId = managerId,
        };
}
